using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vwr.Domain.Entities;

namespace Vwr.Api.Services;

[Route("api/webhooks/in/{source}")]
[ApiController]
public class WebhooksController : ControllerBase
{
    AppDb db;
    public WebhooksController(AppDb _db)
    {
        db = _db;
    }
    [HttpPost]
    public async Task<IActionResult> InboundWebhook(string source)
    {
        using var sr = new StreamReader(Request.Body);
        var raw = await sr.ReadToEndAsync();
        var id = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(raw + source)));


        if (await db.Events.AnyAsync(e => e.Id == id)) return Ok($"/events/{id}");


        db.Events.Add(new EventEntity
        {
            Id = id,
            Source = source,
            Payload = raw,
            ReceivedAt = DateTimeOffset.UtcNow
        });
        await db.SaveChangesAsync();
        return Ok($"/events/{id}");
    }
}