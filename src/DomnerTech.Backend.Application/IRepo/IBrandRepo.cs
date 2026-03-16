using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Brand entity operations.
/// </summary>
public interface IBrandRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new brand.
    /// </summary>
    Task<ObjectId> CreateAsync(BrandEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing brand.
    /// </summary>
    Task UpdateAsync(BrandEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a brand by ID.
    /// </summary>
    Task<BrandEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a brand by slug.
    /// </summary>
    Task<BrandEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active brands.
    /// </summary>
    Task<List<BrandEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a brand.
    /// </summary>
    Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default);
}
