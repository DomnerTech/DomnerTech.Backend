using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class NotificationRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<NotificationEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), INotificationRepo
{
    public async Task<ObjectId> CreateAsync(NotificationEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(NotificationEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<NotificationEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<NotificationEntity>> GetByRecipientAsync(ObjectId recipientId, int limit = 50, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.RecipientId, recipientId);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NotificationEntity>> GetUnreadByRecipientAsync(ObjectId recipientId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.RecipientId, recipientId) &
                     Builders<NotificationEntity>.Filter.Eq(i => i.IsRead, false);
        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(ObjectId recipientId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.RecipientId, recipientId) &
                     Builders<NotificationEntity>.Filter.Eq(i => i.IsRead, false);
        var count = await Collection.CountDocumentsAsync(TenantFilter() & filter, cancellationToken: cancellationToken);
        return (int)count;
    }

    public async Task MarkAsReadAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<NotificationEntity>.Update
            .Set(i => i.IsRead, true)
            .Set(i => i.ReadAt, DateTime.UtcNow)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }

    public async Task MarkAllAsReadAsync(ObjectId recipientId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<NotificationEntity>.Filter.Eq(i => i.RecipientId, recipientId) &
                     Builders<NotificationEntity>.Filter.Eq(i => i.IsRead, false);
        var update = Builders<NotificationEntity>.Update
            .Set(i => i.IsRead, true)
            .Set(i => i.ReadAt, DateTime.UtcNow)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateManyAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
