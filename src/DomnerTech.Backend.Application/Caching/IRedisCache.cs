using DomnerTech.Backend.Application.IRepo;
using System.Text.Json;

namespace DomnerTech.Backend.Application.Caching;

public interface IRedisCache : IBaseRepo
{
    Task SetObjectAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        JsonSerializerOptions? setting = null);

    Task<T?> GetObjectAsync<T>(string key, JsonSerializerOptions? setting = null);

    Task RemoveAsync(string key);
}