using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Domain.ValueObjects;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

/// <summary>
/// Service implementation for inventory management operations.
/// </summary>
public sealed class InventoryService(
    IStockRepo stockRepo,
    IStockMovementRepo stockMovementRepo,
    IStockReservationRepo stockReservationRepo,
    ITenantService tenantService) : IInventoryService
{
    public async Task<bool> AdjustStockAsync(
        ObjectId productId,
        ObjectId warehouseId,
        decimal quantity,
        StockMovementType movementType,
        ObjectId? variantId = null,
        string? batchLotNumber = null,
        string? serialNumber = null,
        ObjectId? referenceId = null,
        string? notes = null,
        CancellationToken cancellationToken = default)
    {
        var stock = await stockRepo.GetByProductAndWarehouseAsync(productId, warehouseId, variantId, cancellationToken) ??
                    new StockEntity
                    {
                        CompanyId = ObjectId.Parse(tenantService.CompanyId),
                        ProductId = productId,
                        WarehouseId = warehouseId,
                        VariantId = variantId,
                        StockLevel = new StockLevelValueObject
                        {
                            QuantityOnHand = 0,
                            ReservedQuantity = 0,
                            ReorderLevel = 10,
                            MaximumLevel = 1000
                        },
                        Batches = [],
                        SerialNumbers = [],
                        CreatedAt = DateTime.UtcNow
                    };

        // Adjust quantity based on movement type
        var previousQuantity = stock.StockLevel.QuantityOnHand;

        switch (movementType)
        {
            case StockMovementType.InitialStock 
                or StockMovementType.PurchaseReceipt 
                or StockMovementType.SaleReturn 
                or StockMovementType.TransferIn 
                or StockMovementType.AdjustmentIncrease:
                stock.StockLevel.QuantityOnHand += quantity;
                break;
            case StockMovementType.Sale 
                or StockMovementType.PurchaseReturn 
                or StockMovementType.TransferOut 
                or StockMovementType.AdjustmentDecrease 
                or StockMovementType.Damage:
            {
                stock.StockLevel.QuantityOnHand -= quantity;

                // Prevent negative stock
                if (stock.StockLevel.QuantityOnHand < 0)
                {
                    stock.StockLevel.QuantityOnHand = 0;
                }

                break;
            }
        }

        // Add or update batch/lot if provided
        if (!string.IsNullOrWhiteSpace(batchLotNumber))
        {
            stock.Batches ??= [];

            var existingBatch = stock.Batches.FirstOrDefault(b => b.Number == batchLotNumber);
            if (existingBatch == null)
            {
                stock.Batches.Add(new BatchLotValueObject
                {
                    Number = batchLotNumber
                });
            }
        }

        // Add serial number if provided
        if (!string.IsNullOrWhiteSpace(serialNumber))
        {
            stock.SerialNumbers ??= [];

            stock.SerialNumbers.Add(new SerialNumberValueObject
            {
                Number = serialNumber
            });
        }

        stock.UpdatedAt = DateTime.UtcNow;
        stock.LastMovementAt = DateTime.UtcNow;

        if (stock.Id == ObjectId.Empty)
        {
            await stockRepo.CreateAsync(stock, cancellationToken);
        }
        else
        {
            await stockRepo.UpdateAsync(stock, cancellationToken);
        }

        // Record stock movement
        var movement = new StockMovementEntity
        {
            CompanyId = ObjectId.Parse(tenantService.CompanyId),
            StockId = stock.Id,
            ProductId = productId,
            WarehouseId = warehouseId,
            MovementType = movementType,
            Quantity = quantity,
            QuantityBefore = previousQuantity,
            QuantityAfter = stock.StockLevel.QuantityOnHand,
            BatchLotNumber = batchLotNumber,
            SerialNumber = serialNumber,
            ReferenceId = referenceId,
            Notes = notes,
            MovementDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await stockMovementRepo.CreateAsync(movement, cancellationToken);

        return true;
    }

    public async Task<ObjectId> ReserveStockAsync(
        ObjectId productId,
        ObjectId warehouseId,
        decimal quantity,
        ObjectId orderId,
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default)
    {
        var stock = await stockRepo.GetByProductAndWarehouseAsync(productId, warehouseId, null, cancellationToken);

        if (stock == null || stock.StockLevel.AvailableQuantity < quantity)
        {
            throw new InvalidOperationException("Insufficient stock available for reservation.");
        }

        // Update stock reserved quantity
        stock.StockLevel.ReservedQuantity += quantity;
        stock.UpdatedAt = DateTime.UtcNow;

        await stockRepo.UpdateAsync(stock, cancellationToken);

        // Create reservation record
        var reservation = new StockReservationEntity
        {
            CompanyId = ObjectId.Parse(tenantService.CompanyId),
            StockId = stock.Id,
            ProductId = productId,
            WarehouseId = warehouseId,
            OrderId = orderId,
            Quantity = quantity,
            ExpiresAt = expiresAt ?? DateTime.UtcNow.AddHours(24),
            IsFulfilled = false,
            IsReleased = false,
            CreatedAt = DateTime.UtcNow
        };

        return await stockReservationRepo.CreateAsync(reservation, cancellationToken);
    }

    public async Task<bool> ReleaseReservationAsync(ObjectId reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await stockReservationRepo.GetByIdAsync(reservationId, cancellationToken);

        if (reservation == null || reservation.IsReleased || reservation.IsFulfilled)
        {
            return false;
        }

        var stock = await stockRepo.GetByProductAndWarehouseAsync(
            reservation.ProductId,
            reservation.WarehouseId,
            null,
            cancellationToken);

        if (stock == null)
        {
            return false;
        }

        // Update stock reserved quantity
        stock.StockLevel.ReservedQuantity -= reservation.Quantity;
        stock.UpdatedAt = DateTime.UtcNow;

        await stockRepo.UpdateAsync(stock, cancellationToken);

        // Update reservation status
        reservation.IsReleased = true;
        reservation.ReleasedAt = DateTime.UtcNow;
        reservation.UpdatedAt = DateTime.UtcNow;
        await stockReservationRepo.UpdateAsync(reservation, cancellationToken);

        return true;
    }

    public async Task<bool> FulfillReservationAsync(ObjectId reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await stockReservationRepo.GetByIdAsync(reservationId, cancellationToken);

        if (reservation == null || reservation.IsReleased || reservation.IsFulfilled)
        {
            return false;
        }

        // Create sale movement
        await AdjustStockAsync(
            reservation.ProductId,
            reservation.WarehouseId,
            reservation.Quantity,
            StockMovementType.Sale,
            null,
            null,
            null,
            reservation.OrderId,
            $"Fulfilled reservation {reservationId}",
            cancellationToken);

        // Update reservation status
        reservation.IsFulfilled = true;
        reservation.FulfilledAt = DateTime.UtcNow;
        reservation.UpdatedAt = DateTime.UtcNow;
        await stockReservationRepo.UpdateAsync(reservation, cancellationToken);

        return true;
    }

    public async Task<decimal> GetAvailableQuantityAsync(
        ObjectId productId,
        ObjectId warehouseId,
        ObjectId? variantId = null,
        CancellationToken cancellationToken = default)
    {
        var stock = await stockRepo.GetByProductAndWarehouseAsync(productId, warehouseId, variantId, cancellationToken);

        return stock?.StockLevel.AvailableQuantity ?? 0;
    }

    public async Task<bool> IsStockAvailableAsync(
        ObjectId productId,
        ObjectId warehouseId,
        decimal requiredQuantity,
        ObjectId? variantId = null,
        CancellationToken cancellationToken = default)
    {
        var availableQuantity = await GetAvailableQuantityAsync(productId, warehouseId, variantId, cancellationToken);

        return availableQuantity >= requiredQuantity;
    }

    public async Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default)
    {
        var expiredReservations = await stockReservationRepo.GetExpiredReservationsAsync(cancellationToken);

        foreach (var reservation in expiredReservations)
        {
            await ReleaseReservationAsync(reservation.Id, cancellationToken);
        }
    }
}
