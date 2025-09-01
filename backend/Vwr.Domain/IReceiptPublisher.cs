using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain;

public interface IReceiptPublisher
{
    Task<string> PublishAsync(byte[] merkleRoot, string metadataUri, CancellationToken ct = default);
}