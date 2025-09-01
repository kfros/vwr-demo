using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain.Entities;

public class EventEntity
{
    public string Id { get; set; } = default!;
    public string Source { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTimeOffset ReceivedAt { get; set; }
}