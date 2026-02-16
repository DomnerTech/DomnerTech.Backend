using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DomnerTech.Backend.Application.IRepo;

public interface IRedisCache : IBaseRepo
{
    Task SetObjectAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task SetObjectAsync<T>(string key,
        T value,
        DistributedCacheEntryOptions options,
        JsonSerializerOptions setting,
        CancellationToken cancellationToken = default);

    Task<T?> GetObjectAsync<T>(string key, CancellationToken cancellationToken = default);
}