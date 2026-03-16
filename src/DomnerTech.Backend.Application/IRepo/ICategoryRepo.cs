using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Category entity operations.
/// </summary>
public interface ICategoryRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<ObjectId> CreateAsync(CategoryEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task UpdateAsync(CategoryEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    Task<CategoryEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a category by slug.
    /// </summary>
    Task<CategoryEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active categories.
    /// </summary>
    Task<List<CategoryEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets child categories by parent ID.
    /// </summary>
    Task<List<CategoryEntity>> GetByParentIdAsync(ObjectId parentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a category.
    /// </summary>
    Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default);
}
