using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for ProductVariant entity operations.
/// </summary>
public interface IProductVariantRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new product variant.
    /// </summary>
    Task<ObjectId> CreateAsync(ProductVariantEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product variant.
    /// </summary>
    Task UpdateAsync(ProductVariantEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a product variant by ID.
    /// </summary>
    Task<ProductVariantEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all variants for a product.
    /// </summary>
    Task<List<ProductVariantEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a variant by SKU.
    /// </summary>
    Task<ProductVariantEntity?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a product variant.
    /// </summary>
    Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default);
}
