using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for leave type operations.
/// </summary>
public interface ILeaveTypeRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new leave type.
    /// </summary>
    Task<ObjectId> CreateAsync(LeaveTypeEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing leave type.
    /// </summary>
    Task UpdateAsync(LeaveTypeEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a leave type.
    /// </summary>
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a leave type by ID.
    /// </summary>
    Task<LeaveTypeEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active leave types.
    /// </summary>
    Task<List<LeaveTypeEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a leave type code already exists.
    /// </summary>
    Task<bool> CodeExistsAsync(string code, ObjectId? excludeId = null, CancellationToken cancellationToken = default);
}
