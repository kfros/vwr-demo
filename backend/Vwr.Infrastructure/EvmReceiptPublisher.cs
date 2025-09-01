using Microsoft.Extensions.Configuration;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;
using Vwr.Domain;

namespace Vwr.Infrastructure
{
    public class EvmReceiptPublisher : IReceiptPublisher
    {
        private readonly Web3 _web3;
        private readonly string _contractAddress;

        public EvmReceiptPublisher(IConfiguration cfg)
        {
            var evm = cfg.GetSection("evm").Get<EvmOptions>() ?? new EvmOptions();
            if (string.IsNullOrWhiteSpace(evm.RpcUrl)) throw new InvalidOperationException("evm:rpcUrl not configured");
            if (string.IsNullOrWhiteSpace(evm.PrivateKey)) throw new InvalidOperationException("evm:privateKey not configured");
            if (string.IsNullOrWhiteSpace(evm.ContractAddress)) throw new InvalidOperationException("evm:contractAddress not configured");


            _web3 = new Web3(new Nethereum.Web3.Accounts.Account(evm.PrivateKey), evm.RpcUrl);
            _contractAddress = evm.ContractAddress;
        }

        // Maps to Solidity: function publish(bytes32 merkleRoot, string metadataURI)
        [Function("publish")]
        public class PublishFunction : FunctionMessage
        {
            [Parameter("bytes32", "merkleRoot", 1)] public byte[] MerkleRoot { get; set; } = Array.Empty<byte>();
            [Parameter("string", "metadataURI", 2)] public string MetadataURI { get; set; } = string.Empty;
        }

        public async Task<string> PublishAsync(byte[] merkleRoot, string metadataUri, CancellationToken ct = default)
        {
            if (merkleRoot.Length != 32) throw new ArgumentException("Merkle root must be 32 bytes", nameof(merkleRoot));


            var handler = _web3.Eth.GetContractTransactionHandler<PublishFunction>();
            var tx = await handler.SendRequestAsync(_contractAddress, new PublishFunction
            {
                MerkleRoot = merkleRoot,
                MetadataURI = metadataUri
            });


            return tx; // tx hash
        }
    }
}