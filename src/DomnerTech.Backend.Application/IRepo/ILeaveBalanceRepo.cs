using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave balance operations.
/// </summary>
public interface ILeaveBalanceRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(LeaveBalanceEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveBalanceEntity entity, CancellationToken cancellationToken = default);
    Task<LeaveBalanceEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<LeaveBalanceEntity?> GetByEmployeeAndTypeAsync(ObjectId employeeId, ObjectId leaveTypeId, int year, CancellationToken cancellationToken = default);
    Task<List<LeaveBalanceEntity>> GetByEmployeeAsync(ObjectId employeeId, int year, CancellationToken cancellationToken = default);
    Task<List<LeaveBalanceEntity>> GetAllByYearAsync(int year, CancellationToken cancellationToken = default);
    Task<bool> HasSufficientBalanceAsync(ObjectId employeeId, ObjectId leaveTypeId, int year, decimal requiredDays, CancellationToken cancellationToken = default);
}
