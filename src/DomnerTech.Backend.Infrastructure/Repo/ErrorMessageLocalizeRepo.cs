using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class ErrorMessageLocalizeRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant,
    IRedisCache redisCache) :
    BaseRepo<ErrorMessageLocalizeEntity>(
        contextFactory.Create(DatabaseNameConstant.DatabaseName).Database,
        tenant),
    IErrorMessageLocalizeRepo
{
    public async Task<string> ResolveAsync(string errorCode, string lang, CancellationToken cancellationToken = default)
    {
        const string cacheKey = ":error-messages-localize";
        var errorMessage = await redisCache.GetObjectAsync<Dictionary<string, Dictionary<string, string>>>(cacheKey);
        if (errorMessage == null)
        {
            var data = await Collection
                .FindAsync(Builders<ErrorMessageLocalizeEntity>.Filter.Empty, cancellationToken: cancellationToken);
            errorMessage = data.ToList(cancellationToken)
                .ToDictionary(k => k.Key, v => v.Messages);
            await redisCache.SetObjectAsync(cacheKey, errorMessage, new CacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
            });
        }

        var message = errorMessage.GetValueOrDefault(errorCode)?.GetValueOrDefault(lang);
        return message ?? errorMessage[ErrorCodes.SystemError][lang];
    }

    public async Task<ObjectId> CreateAsync(ErrorMessageLocalizeEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<ErrorMessageLocalizeEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ErrorMessageLocalizeEntity>.Filter.Eq(i => i.Key, key);
        return await Collection.Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ObjectId> UpdateAsync(ErrorMessageLocalizeEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ErrorMessageLocalizeEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        await redisCache.RemoveAsync(":error-messages-localize");
        return entity.Id;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ErrorMessageLocalizeEntity>.Filter.Eq(i => i.Key, key);
        await Collection.DeleteOneAsync(filter, cancellationToken);
        await redisCache.RemoveAsync(":error-messages-localize");
    }
}