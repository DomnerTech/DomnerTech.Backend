using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class LeaveBlackoutDateRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeaveBlackoutDateEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeaveBlackoutDateRepo
{
    public async Task<ObjectId> CreateAsync(LeaveBlackoutDateEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeaveBlackoutDateEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<LeaveBlackoutDateEntity>.Update
            .Set(i => i.IsDeleted, true)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }

    public async Task<LeaveBlackoutDateEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.Id, id) &
                     Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeaveBlackoutDateEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsBlackoutDateAsync(DateTime startDate, DateTime endDate, string? department = null, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     (Builders<LeaveBlackoutDateEntity>.Filter.Lte(i => i.StartDate, endDate) &
                      Builders<LeaveBlackoutDateEntity>.Filter.Gte(i => i.EndDate, startDate));

        // If department specified, check if blackout applies to it or to all departments
        if (!string.IsNullOrEmpty(department))
        {
            filter &= (Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.Departments, null) |
                      Builders<LeaveBlackoutDateEntity>.Filter.AnyEq(i => i.Departments, department));
        }

        var count = await Collection.CountDocumentsAsync(TenantFilter() & filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<List<LeaveBlackoutDateEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<LeaveBlackoutDateEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     (Builders<LeaveBlackoutDateEntity>.Filter.Lte(i => i.StartDate, endDate) &
                      Builders<LeaveBlackoutDateEntity>.Filter.Gte(i => i.EndDate, startDate));

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.StartDate)
            .ToListAsync(cancellationToken);
    }
}
