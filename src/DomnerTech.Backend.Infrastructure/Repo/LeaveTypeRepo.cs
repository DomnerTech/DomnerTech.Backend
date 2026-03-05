using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for leave type operations.
/// </summary>
public sealed class LeaveTypeRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeaveTypeEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeaveTypeRepo
{
    public async Task<ObjectId> CreateAsync(LeaveTypeEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeaveTypeEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveTypeEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveTypeEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<LeaveTypeEntity>.Update
            .Set(i => i.IsDeleted, true)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }

    public async Task<LeaveTypeEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveTypeEntity>.Filter.Eq(i => i.Id, id) &
                     Builders<LeaveTypeEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeaveTypeEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveTypeEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeaveTypeEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(string code, ObjectId? excludeId = null, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveTypeEntity>.Filter.Eq(i => i.Code, code.ToUpperInvariant()) &
                     Builders<LeaveTypeEntity>.Filter.Eq(i => i.IsDeleted, false);

        if (excludeId.HasValue)
        {
            filter &= Builders<LeaveTypeEntity>.Filter.Ne(i => i.Id, excludeId.Value);
        }

        var count = await Collection.CountDocumentsAsync(TenantFilter() & filter, cancellationToken: cancellationToken);
        return count > 0;
    }
}
