using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vwr.Domain.Entities;

namespace Vwr.Infrastructure;

public static class DbSetup
{
    public static IServiceCollection AddAppDb(this IServiceCollection services, string? cs)
    {
        services.AddDbContext<AppDb>(o => o.UseNpgsql(cs));
        return services;
    }
}
