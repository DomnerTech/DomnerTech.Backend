using DomnerTech.Backend.Application;
using DomnerTech.Backend.Infrastructure.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Infrastructure.Caching;
public static class Extensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddRedis(appSettings);
        return services;
    }
}
