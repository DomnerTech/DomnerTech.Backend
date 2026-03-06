using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class AuditLogRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<AuditLogEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IAuditLogRepo
{
    public async Task<ObjectId> CreateAsync(AuditLogEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<List<AuditLogEntity>> GetByEntityAsync(string entityType, ObjectId entityId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AuditLogEntity>.Filter.Eq(i => i.EntityType, entityType) &
                     Builders<AuditLogEntity>.Filter.Eq(i => i.EntityId, entityId);

        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AuditLogEntity>> GetByUserAsync(ObjectId userId, int limit = 100, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AuditLogEntity>.Filter.Eq(i => i.UserId, userId);

        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AuditLogEntity>> GetByActionAsync(string action, int limit = 100, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AuditLogEntity>.Filter.Eq(i => i.Action, action);

        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AuditLogEntity>> GetRecentAsync(int limit = 100, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(TenantFilter())
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }
}
