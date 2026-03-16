using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service interface for product catalog operations.
/// </summary>
public interface IProductService : IBaseService
{
    /// <summary>
    /// Generates a unique SKU for a product.
    /// </summary>
    Task<string> GenerateSkuAsync(ObjectId categoryId, string? prefix = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates product prices across all currencies.
    /// </summary>
    Task<bool> ValidatePricesAsync(ProductEntity product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates product status based on stock availability.
    /// </summary>
    Task UpdateProductStatusAsync(ObjectId productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a product can be deleted.
    /// </summary>
    Task<bool> CanDeleteProductAsync(ObjectId productId, CancellationToken cancellationToken = default);
}
