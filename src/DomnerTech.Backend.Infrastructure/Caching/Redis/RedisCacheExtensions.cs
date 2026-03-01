using DomnerTech.Backend.Application.Caching;

namespace DomnerTech.Backend.Infrastructure.Caching.Redis;

public static class RedisCacheExtensions
{
    public static async Task<T?> RedisFallbackAsync<T>(
        this IRedisCache redis,
        string key,
        CacheEntryOptions options,
        Func<Task<T>> fallback)
    {
        var value = await redis.GetObjectAsync<T>(key);

        if (value is not null) return value;

        value = await fallback.Invoke();

        if (value is not null)
        {
            await redis.SetObjectAsync(key, value, options);
        }

        return value;
    }
}
