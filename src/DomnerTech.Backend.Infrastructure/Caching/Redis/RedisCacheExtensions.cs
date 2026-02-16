using DomnerTech.Backend.Application.IRepo;
using Microsoft.Extensions.Caching.Distributed;

namespace DomnerTech.Backend.Infrastructure.Caching.Redis;

public static class RedisCacheExtensions
{
    extension(IRedisCache redis)
    {
        public async Task<T?> RedisFallbackAsync<T>(string key,
            DistributedCacheEntryOptions options,
            Func<Task<T>> fallback,
            CancellationToken cancellationToken = default)
        {
            var value = await redis.GetObjectAsync<T>(key, cancellationToken);

            if (value is not null) return value;

            value = await fallback.Invoke();

            if (value is not null)
            {
                await redis.SetObjectAsync(key, value, options, cancellationToken);
            }

            return value;
        }

        public async Task<T?> RedisFallbackAsync<T>(string key,
            Func<Task<T>> fallback,
            CancellationToken cancellationToken = default)
        {
            var value = await redis.GetObjectAsync<T>(key, cancellationToken);

            if (value is not null) return value;

            value = await fallback.Invoke();

            if (value is not null)
            {
                await redis.SetObjectAsync(key, value, cancellationToken: cancellationToken);
            }

            return value;
        }
    }
}
