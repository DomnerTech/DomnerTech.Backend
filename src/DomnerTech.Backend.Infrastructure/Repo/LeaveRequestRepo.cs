using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class LeaveRequestRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeaveRequestEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeaveRequestRepo
{
    public async Task<ObjectId> CreateAsync(LeaveRequestEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeaveRequestEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<LeaveRequestEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.Id, id) &
                     Builders<LeaveRequestEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeaveRequestEntity>> GetByEmployeeAsync(ObjectId employeeId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.EmployeeId, employeeId) &
                     Builders<LeaveRequestEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortByDescending(x => x.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveRequestEntity>> GetByStatusAsync(LeaveRequestStatus status, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.Status, status) &
                     Builders<LeaveRequestEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingRequestAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, ObjectId? excludeId = null, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.EmployeeId, employeeId) &
                     Builders<LeaveRequestEntity>.Filter.Ne(i => i.Status, LeaveRequestStatus.Rejected) &
                     Builders<LeaveRequestEntity>.Filter.Ne(i => i.Status, LeaveRequestStatus.Cancelled) &
                     Builders<LeaveRequestEntity>.Filter.Eq(i => i.IsDeleted, false) &
                     (
                         (Builders<LeaveRequestEntity>.Filter.Lte("Period.StartDate", endDate) &
                          Builders<LeaveRequestEntity>.Filter.Gte("Period.EndDate", startDate))
                     );

        if (excludeId.HasValue)
        {
            filter &= Builders<LeaveRequestEntity>.Filter.Ne(i => i.Id, excludeId.Value);
        }

        var count = await Collection.CountDocumentsAsync(TenantFilter() & filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<List<LeaveRequestEntity>> GetPendingApprovalsAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveRequestEntity>.Filter.Eq(i => i.Status, LeaveRequestStatus.Pending) &
                     Builders<LeaveRequestEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.SubmittedAt)
            .ToListAsync(cancellationToken);
    }
}
