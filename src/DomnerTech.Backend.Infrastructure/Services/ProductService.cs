using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

/// <summary>
/// Service implementation for product catalog operations.
/// </summary>
public sealed class ProductService(
    IProductRepo productRepo,
    ICategoryRepo categoryRepo,
    IStockRepo stockRepo) : IProductService
{
    public async Task<string> GenerateSkuAsync(ObjectId categoryId, string? prefix = null, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepo.GetByIdAsync(categoryId, cancellationToken);
        var categoryPrefix = prefix ?? category?.Slug ?? "PROD";
        
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        
        var sku = $"{categoryPrefix.ToUpper()}-{timestamp}-{random}";
        
        // Ensure uniqueness
        var existingProduct = await productRepo.GetBySkuAsync(sku, cancellationToken);
        if (existingProduct != null)
        {
            // Retry with new random number
            random = new Random().Next(1000, 9999);
            sku = $"{categoryPrefix.ToUpper()}-{timestamp}-{random}";
        }
        
        return sku;
    }

    public Task<bool> ValidatePricesAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        if (product.Prices == null || product.Prices.Count == 0)
        {
            return Task.FromResult(false);
        }

        // Check that each price has amount greater than 0
        var hasValidPrices = product.Prices.All(p => p.Amount > 0);

        // Check that at least one retail price exists for each currency
        var currencies = product.Prices.Select(p => p.Currency).Distinct();
        var hasRetailPrices = currencies.All(currency => 
            product.Prices.Any(p => p.Currency == currency && p.PriceType == PriceType.Retail));

        return Task.FromResult(hasValidPrices && hasRetailPrices);
    }

    public async Task UpdateProductStatusAsync(ObjectId productId, CancellationToken cancellationToken = default)
    {
        var product = await productRepo.GetByIdAsync(productId, cancellationToken);
        if (product == null || !product.TrackInventory)
        {
            return;
        }

        var stockItems = await stockRepo.GetByProductIdAsync(productId, cancellationToken);
        var totalAvailableQuantity = stockItems.Sum(s => s.StockLevel.AvailableQuantity);

        var newStatus = totalAvailableQuantity <= 0 ? ProductStatus.OutOfStock : ProductStatus.Active;
        
        if (product.Status != newStatus)
        {
            product.Status = newStatus;
            product.UpdatedAt = DateTime.UtcNow;
            await productRepo.UpdateAsync(product, cancellationToken);
        }
    }

    public async Task<bool> CanDeleteProductAsync(ObjectId productId, CancellationToken cancellationToken = default)
    {
        var stockItems = await stockRepo.GetByProductIdAsync(productId, cancellationToken);

        // Cannot delete if there's any stock quantity
        var hasStock = stockItems.Any(s => s.StockLevel.QuantityOnHand > 0);

        return !hasStock;
    }
}
