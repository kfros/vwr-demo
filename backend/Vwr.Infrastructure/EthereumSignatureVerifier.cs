using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Signer;
using Vwr.Domain;

namespace Vwr.Infrastructure;

public class EthereumSignatureVerifier : ISignatureVerifier
{
    public Task<bool> VerifyAsync(SiweRequest r, CancellationToken ct = default)
    {
        var signer = new EthereumMessageSigner();
        var recovered = signer.EncodeUTF8AndEcRecover(r.Message, r.Signature);
        return Task.FromResult(string.Equals(recovered, r.Address, StringComparison.OrdinalIgnoreCase));
    }
}