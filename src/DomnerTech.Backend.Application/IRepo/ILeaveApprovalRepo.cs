using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave approval operations.
/// </summary>
public interface ILeaveApprovalRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(LeaveApprovalEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveApprovalEntity entity, CancellationToken cancellationToken = default);
    Task<LeaveApprovalEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<List<LeaveApprovalEntity>> GetByLeaveRequestAsync(ObjectId leaveRequestId, CancellationToken cancellationToken = default);
    Task<List<LeaveApprovalEntity>> GetPendingByApproverAsync(ObjectId approverId, CancellationToken cancellationToken = default);
    Task<LeaveApprovalEntity?> GetCurrentPendingApprovalAsync(ObjectId leaveRequestId, CancellationToken cancellationToken = default);
}
