using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vwr.Domain.Entities;

namespace Vwr.Workers;

public class WebhookRetryWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    public WebhookRetryWorker(IServiceProvider sp) => _sp = sp;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDb>();
            var count = await db.Events.CountAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
