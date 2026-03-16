using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Events;
using DomnerTech.Backend.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

/// <summary>
/// Service implementation for stock alert notifications.
/// </summary>
public sealed class StockAlertService(
    IStockRepo stockRepo,
    IProductRepo productRepo,
    IWarehouseRepo warehouseRepo,
    ILogger<StockAlertService> logger) : IStockAlertService
{
    public async Task CheckLowStockAlertsAsync(CancellationToken cancellationToken = default)
    {
        var lowStockItems = await stockRepo.GetLowStockItemsAsync(null, cancellationToken);
        
        logger.LogInformation("Found {Count} low stock items", lowStockItems.Count);
        
        foreach (var stock in lowStockItems)
        {
            await SendLowStockAlertAsync(stock, cancellationToken);
        }
    }

    public async Task SendLowStockAlertAsync(StockEntity stock, CancellationToken cancellationToken = default)
    {
        var product = await productRepo.GetByIdAsync(stock.ProductId, cancellationToken);
        var warehouse = await warehouseRepo.GetByIdAsync(stock.WarehouseId, cancellationToken);
        
        if (product == null || warehouse == null)
        {
            logger.LogWarning("Cannot send alert for stock {StockId} - product or warehouse not found", stock.Id);
            return;
        }

        // Create domain event for low stock alert
        var alertEvent = new LowStockAlertEvent
        {
            StockId = stock.Id,
            CompanyId = stock.CompanyId,
            ProductId = stock.ProductId,
            ProductName = product.Name.GetValueOrDefault("en", "Unknown"),
            ProductSku = product.Sku.Code,
            WarehouseId = stock.WarehouseId,
            WarehouseName = warehouse.Name,
            AvailableQuantity = stock.StockLevel.AvailableQuantity,
            ReorderLevel = stock.StockLevel.ReorderLevel
        };

        // TODO: Publish event to Kafka for notification processing
        // await eventPublisher.PublishAsync("stock.lowStockAlert", alertEvent);
        
        logger.LogInformation(
            "Low stock alert: Product {ProductName} ({Sku}) at warehouse {WarehouseName} - Current: {Current}, Reorder Level: {ReorderLevel}",
            product.Name.GetValueOrDefault("en", "Unknown"),
            product.Sku.Code,
            warehouse.Name,
            stock.StockLevel.AvailableQuantity,
            stock.StockLevel.ReorderLevel);
    }

    public async Task CheckExpiryAlertsAsync(CancellationToken cancellationToken = default)
    {
        var allStocks = await stockRepo.GetLowStockItemsAsync(null, cancellationToken);
        var expiringBatches = new List<(StockEntity Stock, BatchLotValueObject Batch)>();
        var expiryThreshold = DateTime.UtcNow.AddDays(30);

        foreach (var stock in allStocks)
        {
            if (stock.Batches == null || stock.Batches.Count == 0)
            {
                continue;
            }

            foreach (var batch in stock.Batches)
            {
                if (batch.ExpiryDate.HasValue && batch.ExpiryDate.Value <= expiryThreshold)
                {
                    expiringBatches.Add((stock, batch));
                }
            }
        }

        logger.LogInformation("Found {Count} expiring batches", expiringBatches.Count);

        foreach (var (stock, batch) in expiringBatches)
        {
            var product = await productRepo.GetByIdAsync(stock.ProductId, cancellationToken);
            var warehouse = await warehouseRepo.GetByIdAsync(stock.WarehouseId, cancellationToken);

            if (product != null && warehouse != null)
            {
                logger.LogWarning(
                    "Expiry alert: Product {ProductName} ({Sku}) at warehouse {WarehouseName} - Batch {BatchNumber} expires on {ExpiryDate}",
                    product.Name.GetValueOrDefault("en", "Unknown"),
                    product.Sku.Code,
                    warehouse.Name,
                    batch.Number,
                    batch.ExpiryDate);
            }
        }
    }

    public async Task<List<StockEntity>> GetLowStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default)
    {
        return await stockRepo.GetLowStockItemsAsync(warehouseId, cancellationToken);
    }
}
