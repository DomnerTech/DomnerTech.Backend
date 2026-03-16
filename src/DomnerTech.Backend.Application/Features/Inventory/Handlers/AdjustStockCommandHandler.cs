using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Inventory.Handlers;

/// <summary>
/// Handler for adjusting stock quantity.
/// </summary>
public sealed class AdjustStockCommandHandler(
    ILogger<AdjustStockCommandHandler> logger,
    IInventoryService inventoryService,
    IProductRepo productRepo,
    IWarehouseRepo warehouseRepo) : IRequestHandler<AdjustStockCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var productId = dto.ProductId.ToObjectId();
            var warehouseId = dto.WarehouseId.ToObjectId();
            var variantId = dto.VariantId?.ToObjectId();

            // Validate product exists
            var product = await productRepo.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Product not found"
                    }
                };
            }

            // Validate warehouse exists
            var warehouse = await warehouseRepo.GetByIdAsync(warehouseId, cancellationToken);
            if (warehouse == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Warehouse not found"
                    }
                };
            }

            // Determine movement type based on quantity and reason
            var movementType = dto.Quantity > 0
                ? StockMovementType.AdjustmentIncrease
                : StockMovementType.AdjustmentDecrease;

            // Adjust stock
            var result = await inventoryService.AdjustStockAsync(
                productId,
                warehouseId,
                dto.Quantity,
                movementType,
                variantId,
                dto.BatchLotNumber,
                dto.SerialNumber,
                dto.ReferenceId?.ToObjectId(),
                dto.Notes,
                cancellationToken);

            if (result)
            {
                logger.LogInformation(
                    "Stock adjusted successfully for product {ProductId} in warehouse {WarehouseId}, Quantity: {Quantity}",
                    productId, warehouseId, dto.Quantity);
            }

            return new BaseResponse<bool>
            {
                Data = result
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error adjusting stock: {Error}", e.Message);
        }

        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
