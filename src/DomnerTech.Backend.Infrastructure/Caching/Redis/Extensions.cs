using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Caching;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DomnerTech.Backend.Infrastructure.Caching.Redis;

public static class Extensions
{
    /// <summary>
    /// Registers Redis caching services with a single <see cref="IConnectionMultiplexer"/> instance.
    /// </summary>
    /// <remarks>
    /// Creates one singleton connection to Redis and shares it between IDistributedCache
    /// and IRedisCache implementations, ensuring efficient connection reuse.
    /// </remarks>
    /// <param name="services">The service collection to register services with.</param>
    /// <param name="appSettings">Application settings containing Redis configuration.</param>
    /// <returns>The modified service collection for method chaining.</returns>
    public static IServiceCollection AddRedis(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var configOption = new ConfigurationOptions
            {
                Ssl = appSettings.Redis.Ssl,
                ClientName = appSettings.Redis.ClientName
            };

            foreach (var redisConfigUrl in appSettings.Redis.EndPoints) configOption.EndPoints.Add(redisConfigUrl);
            configOption.User = appSettings.Redis.Username;
            configOption.Password = appSettings.Redis.Password;
            configOption.ConnectTimeout = appSettings.Redis.ConnectTimeout;
            var mux = ConnectionMultiplexer.Connect(configOption);
            mux.UseElasticApm();
            return mux;
        });
        // Register custom Redis cache wrapper
        services.AddSingleton<IRedisCache, RedisCache>();
        return services;
    }
}
