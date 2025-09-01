using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Vwr.Domain;
using Vwr.Domain.Entities;
using Vwr.Infrastructure;

namespace Vwr.Workers;

public class MerkleBatchWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    public MerkleBatchWorker(IServiceProvider sp) => _sp = sp;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDb>();
            var publisher = scope.ServiceProvider.GetRequiredService<IReceiptPublisher>();


            var last50 = await db.Events.OrderByDescending(e => e.ReceivedAt).Take(50).ToListAsync(stoppingToken);
            if (last50.Count > 0)
            {
                var leaves = last50.Select(e => System.Text.Encoding.UTF8.GetBytes(e.Id)).ToArray();
                var root = Merkle.Root(leaves);
                var tx = await publisher.PublishAsync(root, "about:blank", stoppingToken);
                db.Receipts.Add(new ReceiptEntity
                {
                    Id = Guid.NewGuid(),
                    MerkleRoot = Convert.ToHexString(root),
                    TxHash = tx,
                    MetadataUri = "about:blank",
                    PublishedAt = DateTimeOffset.UtcNow
                });
                await db.SaveChangesAsync(stoppingToken);
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}