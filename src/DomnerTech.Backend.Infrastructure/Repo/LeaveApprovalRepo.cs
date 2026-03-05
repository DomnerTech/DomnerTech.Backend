using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class LeaveApprovalRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeaveApprovalEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeaveApprovalRepo
{
    public async Task<ObjectId> CreateAsync(LeaveApprovalEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeaveApprovalEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveApprovalEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<LeaveApprovalEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveApprovalEntity>.Filter.Eq(i => i.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeaveApprovalEntity>> GetByLeaveRequestAsync(ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveApprovalEntity>.Filter.Eq(i => i.LeaveRequestId, leaveRequestId);
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveApprovalEntity>> GetPendingByApproverAsync(ObjectId approverId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveApprovalEntity>.Filter.Eq(i => i.ApproverId, approverId) &
                     Builders<LeaveApprovalEntity>.Filter.Eq(i => i.Status, LeaveRequestStatus.Pending);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<LeaveApprovalEntity?> GetCurrentPendingApprovalAsync(ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveApprovalEntity>.Filter.Eq(i => i.LeaveRequestId, leaveRequestId) &
                     Builders<LeaveApprovalEntity>.Filter.Eq(i => i.Status, LeaveRequestStatus.Pending);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.SequenceOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
