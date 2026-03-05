using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class LeavePolicyRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeavePolicyEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeavePolicyRepo
{
    public async Task<ObjectId> CreateAsync(LeavePolicyEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeavePolicyEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<LeavePolicyEntity>.Update
            .Set(i => i.IsDeleted, true)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }

    public async Task<LeavePolicyEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.Id, id) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeavePolicyEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     Builders<LeavePolicyEntity>.Filter.Lte(i => i.EffectiveFrom, now) &
                     (Builders<LeavePolicyEntity>.Filter.Eq(i => i.EffectiveTo, null) |
                      Builders<LeavePolicyEntity>.Filter.Gte(i => i.EffectiveTo, now));

        return await Collection.Find(TenantFilter() & filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<LeavePolicyEntity?> GetByLeaveTypeIdAsync(ObjectId leaveTypeId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.LeaveTypeId, leaveTypeId) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     Builders<LeavePolicyEntity>.Filter.Lte(i => i.EffectiveFrom, now) &
                     (Builders<LeavePolicyEntity>.Filter.Eq(i => i.EffectiveTo, null) |
                      Builders<LeavePolicyEntity>.Filter.Gte(i => i.EffectiveTo, now));

        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<LeavePolicyEntity?> GetDefaultPolicyAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = Builders<LeavePolicyEntity>.Filter.Eq(i => i.LeaveTypeId, null) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeavePolicyEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     Builders<LeavePolicyEntity>.Filter.Lte(i => i.EffectiveFrom, now) &
                     (Builders<LeavePolicyEntity>.Filter.Eq(i => i.EffectiveTo, null) |
                      Builders<LeavePolicyEntity>.Filter.Gte(i => i.EffectiveTo, now));

        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
