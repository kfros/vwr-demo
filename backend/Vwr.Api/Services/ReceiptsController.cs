using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls.Crypto;
using System.Security.Cryptography;
using System.Text;
using Vwr.Domain;
using Vwr.Domain.Entities;
using Vwr.Infrastructure;

namespace Vwr.Api.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly AppDb _db;
        private readonly IReceiptPublisher _publisher;
        public ReceiptsController(AppDb db, IReceiptPublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int take = 50)
        {
            take = Math.Clamp(take, 1, 200);
            var items = await _db.Receipts
                .OrderByDescending(x => x.PublishedAt)
                .Take(take)
                .Select(x => new {
                    x.Id,
                    x.PublishedAt,
                    x.MerkleRoot,
                    x.TxHash,
                    x.MetadataUri
                })
                .ToListAsync();

            return Ok(items);
        }

        // Get last N events, count merkleroot digest and publish
        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromQuery] int take = 16)
        {
            // 1) fetch last N events from DB
            var items = await _db.Events
                .OrderByDescending(e => e.ReceivedAt)
                .Take(Math.Clamp(take, 1, 256))
                .ToListAsync();

            if (items.Count == 0)
                return BadRequest(new { error = "no events" });

            // 2) leaves = sha256(payload)
            var leaves = items
                .Select(e => SHA256.HashData(Encoding.UTF8.GetBytes(e.Payload)))
                .ToList();

            var rootBytes = Merkle.Root(leaves);
            var rootHex = Merkle.ToHex32(rootBytes);


            // 3) simple Merkle (repeat last element for odd quantity)
            static byte[] Keccak(byte[] a, byte[] b)
            {
                // we can leave SHA256 for concistency, but usually root for EVM is calculated by keccak256
                using var sha = SHA256.Create();
                var buf = new byte[a.Length + b.Length];
                Buffer.BlockCopy(a, 0, buf, 0, a.Length);
                Buffer.BlockCopy(b, 0, buf, a.Length, b.Length);
                return sha.ComputeHash(buf);
            }

            var level = leaves.Select(x => x).ToList();
            while (level.Count > 1)
            {
                var next = new List<byte[]>();
                for (int i = 0; i < level.Count; i += 2)
                {
                    var left = level[i];
                    var right = (i + 1 < level.Count) ? level[i + 1] : level[i];
                    next.Add(Keccak(left, right));
                }
                level = next;
            }
            var root = level[0];                  // 32 bytes
            var meta = $"ipfs://example/metadata.json"; // you can put here CID with batch description

            // 4) publish via Nethereum
            var tx = await _publisher.PublishAsync(root, meta);

            var receipt = new ReceiptEntity
            {
                MerkleRoot = rootHex,
                TxHash = tx,
                MetadataUri = meta,
                PublishedAt = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid()
            };
            _db.Receipts.Add(receipt);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                count = items.Count,
                merkleRoot = Convert.ToHexString(root),
                metadataUri = meta,
                txHash = tx
            });
        }
    }
}
