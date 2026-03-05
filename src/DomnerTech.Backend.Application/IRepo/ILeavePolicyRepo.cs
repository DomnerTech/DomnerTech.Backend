using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave policy operations.
/// </summary>
public interface ILeavePolicyRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(LeavePolicyEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeavePolicyEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<LeavePolicyEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<List<LeavePolicyEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<LeavePolicyEntity?> GetByLeaveTypeIdAsync(ObjectId leaveTypeId, CancellationToken cancellationToken = default);
    Task<LeavePolicyEntity?> GetDefaultPolicyAsync(CancellationToken cancellationToken = default);
}
