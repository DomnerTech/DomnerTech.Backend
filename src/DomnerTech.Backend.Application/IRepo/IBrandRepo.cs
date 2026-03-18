using DomnerTech.Backend.Application.DTOs.Brands;
using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Brand entity operations.
/// </summary>
public interface IBrandRepo : IBaseRepo, IBasePagedRepo<BrandEntity, BrandDto>
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
    /// Gets all active brands.
    /// </summary>
    Task<List<BrandEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
