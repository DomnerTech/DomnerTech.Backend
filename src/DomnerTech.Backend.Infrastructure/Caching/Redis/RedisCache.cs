using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace DomnerTech.Backend.Infrastructure.Caching.Redis;

public class RedisCache(
    ILogger<RedisCache> logger,
    IConnectionMultiplexer connectionMultiplexer,
    AppSettings appSettings) : IRedisCache
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _instanceName = appSettings.Redis.InstanceName;

    private const string DataFieldName = "data";
    private const string ExpiryFieldName = "expiry";
    public async Task SetObjectAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        JsonSerializerOptions? setting = null)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value, setting ?? DefaultJsonSerializerSettings.SnakeCase);
            var prefixedKey = new RedisKey($"{_instanceName}{key}");
            var expiry = options?.AbsoluteExpiration.HasValue == true
                ? options.AbsoluteExpiration.Value.UtcDateTime - DateTime.UtcNow
                : options?.SlidingExpiration ?? TimeSpan.FromHours(1);

            var hashEntries = new HashEntry[]
            {
                new(DataFieldName, json),
                new(ExpiryFieldName, DateTimeOffset.UtcNow.Add(expiry).ToUnixTimeSeconds())
            };

            await _database.HashSetAsync(prefixedKey, hashEntries);
            await _database.KeyExpireAsync(prefixedKey, expiry);
        }
        catch (RedisConnectionException e)
        {
            logger.LogError(e, "redis: failed setting - {@Message}", e.Message);
        }
        catch (RedisTimeoutException e)
        {
            logger.LogError(e, "redis: failed setting - {@Message}", e.Message);
        }
        catch (JsonException e)
        {
            logger.LogError(e, "redis: failed setting - {@Message}", e.Message);
        }
    }

    public async Task<T?> GetObjectAsync<T>(string key, JsonSerializerOptions? setting = null)
    {
        try
        {
            var prefixedKey = new RedisKey($"{_instanceName}{key}");
            var value = await _database.HashGetAsync(prefixedKey, DataFieldName);
            return value.HasValue ? JsonConvert.DeserializeObject<T>(value.ToString(), setting ?? DefaultJsonSerializerSettings.SnakeCase) : default;
        }
        catch (Exception e)
        {
            logger.LogError(e, "redis: failed getting - {@Message}", e.Message);
        }

        return default;
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            var prefixedKey = new RedisKey($"{_instanceName}{key}");
            await _database.KeyDeleteAsync(prefixedKey);
        }
        catch (RedisConnectionException e)
        {
            logger.LogError(e, "redis: failed removed - {@Message}", e.Message);
        }
        catch (RedisTimeoutException e)
        {
            logger.LogError(e, "redis: failed removed - {@Message}", e.Message);
        }
        catch (JsonException e)
        {
            logger.LogError(e, "redis: failed removed - {@Message}", e.Message);
        }
    }
}
