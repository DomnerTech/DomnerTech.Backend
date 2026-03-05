using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave request operations.
/// </summary>
public interface ILeaveRequestRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(LeaveRequestEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveRequestEntity entity, CancellationToken cancellationToken = default);
    Task<LeaveRequestEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<List<LeaveRequestEntity>> GetByEmployeeAsync(ObjectId employeeId, CancellationToken cancellationToken = default);
    Task<List<LeaveRequestEntity>> GetByStatusAsync(LeaveRequestStatus status, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingRequestAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, ObjectId? excludeId = null, CancellationToken cancellationToken = default);
    Task<List<LeaveRequestEntity>> GetPendingApprovalsAsync(CancellationToken cancellationToken = default);
}
