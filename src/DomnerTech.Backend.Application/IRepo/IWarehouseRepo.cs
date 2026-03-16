using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Warehouse entity operations.
/// </summary>
public interface IWarehouseRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new warehouse.
    /// </summary>
    Task<ObjectId> CreateAsync(WarehouseEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing warehouse.
    /// </summary>
    Task UpdateAsync(WarehouseEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a warehouse by ID.
    /// </summary>
    Task<WarehouseEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a warehouse by code.
    /// </summary>
    Task<WarehouseEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active warehouses.
    /// </summary>
    Task<List<WarehouseEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the default warehouse.
    /// </summary>
    Task<WarehouseEntity?> GetDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a warehouse.
    /// </summary>
    Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default);
}
