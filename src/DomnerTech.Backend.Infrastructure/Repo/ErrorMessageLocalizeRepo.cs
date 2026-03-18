using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class ErrorMessageLocalizeRepo(
    IMongoDbContextFactory contextFactory,
    IRedisCache redisCache) :
    BaseRepo<ErrorMessageLocalizeEntity>(
        contextFactory.Create(DatabaseNameConstant.DatabaseName).Database),
    IErrorMessageLocalizeRepo
{
    private const string CacheKey = ":error-messages-localize";
    public async Task<string> ResolveAsync(string errorCode, string lang, CancellationToken cancellationToken = default)
    {
        var errorMessage = await redisCache.GetObjectAsync<Dictionary<string, Dictionary<string, string>>>(CacheKey);
        if (errorMessage == null)
        {
            var data = await Collection
                .FindAsync(Builders<ErrorMessageLocalizeEntity>.Filter.Empty, cancellationToken: cancellationToken);
            errorMessage = (await data.ToListAsync(cancellationToken))
                .ToDictionary(k => k.Key, v => v.Messages);
            if (errorMessage.Count > 0)
            {
                await redisCache.SetObjectAsync(CacheKey, errorMessage, new CacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
                });
            }
        }

        var message = errorMessage.GetValueOrDefault(errorCode)?.GetValueOrDefault(lang);
        return message ?? errorMessage[ErrorCodes.SystemError][lang];
    }

    public async Task<ObjectId> CreateAsync(ErrorMessageLocalizeEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        await redisCache.RemoveAsync(CacheKey);
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
        await redisCache.RemoveAsync(CacheKey);
        return entity.Id;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ErrorMessageLocalizeEntity>.Filter.Eq(i => i.Key, key);
        await Collection.DeleteOneAsync(filter, cancellationToken);
        await redisCache.RemoveAsync(CacheKey);
    }
}