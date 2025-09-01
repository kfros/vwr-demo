using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain;

public interface ISignatureVerifier
{
    Task<bool> VerifyAsync(SiweRequest r, CancellationToken ct = default);
}