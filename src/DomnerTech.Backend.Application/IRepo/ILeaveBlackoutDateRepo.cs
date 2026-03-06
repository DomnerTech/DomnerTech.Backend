using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave blackout date operations.
/// </summary>
public interface ILeaveBlackoutDateRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(LeaveBlackoutDateEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveBlackoutDateEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<LeaveBlackoutDateEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<List<LeaveBlackoutDateEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> IsBlackoutDateAsync(DateTime startDate, DateTime endDate, string? department = null, CancellationToken cancellationToken = default);
    Task<List<LeaveBlackoutDateEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
