using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain.Entities;

public class ReceiptEntity
{
    public Guid Id { get; set; }
    public string MerkleRoot { get; set; } = default!;
    public string TxHash { get; set; } = default!;
    public DateTimeOffset PublishedAt { get; set; }
    public string MetadataUri { get; set; } = default!;
}