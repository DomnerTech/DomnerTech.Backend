using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

/// <summary>
/// Repository interface for Product entity operations.
/// </summary>
public interface IProductRepo : IBaseRepo
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    Task<ObjectId> CreateAsync(ProductEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    Task UpdateAsync(ProductEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a product by ID.
    /// </summary>
    Task<ProductEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a product by SKU.
    /// </summary>
    Task<ProductEntity?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a product by barcode.
    /// </summary>
    Task<ProductEntity?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets products by category ID.
    /// </summary>
    Task<List<ProductEntity>> GetByCategoryIdAsync(ObjectId categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets products by brand ID.
    /// </summary>
    Task<List<ProductEntity>> GetByBrandIdAsync(ObjectId brandId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets products by status.
    /// </summary>
    Task<List<ProductEntity>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches products by name or SKU.
    /// </summary>
    Task<List<ProductEntity>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a product.
    /// </summary>
    Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default);
}
