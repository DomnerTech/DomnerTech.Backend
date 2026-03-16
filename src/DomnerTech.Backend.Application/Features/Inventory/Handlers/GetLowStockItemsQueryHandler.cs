using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Inventory.Handlers;

/// <summary>
/// Handler for getting low stock items.
/// </summary>
public sealed class GetLowStockItemsQueryHandler(
    ILogger<GetLowStockItemsQueryHandler> logger,
    IStockRepo stockRepo,
    IProductRepo productRepo,
    IWarehouseRepo warehouseRepo) : IRequestHandler<GetLowStockItemsQuery, BaseResponse<List<StockDto>>>
{
    public async Task<BaseResponse<List<StockDto>>> Handle(GetLowStockItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouseId = string.IsNullOrWhiteSpace(request.WarehouseId)
                ? (MongoDB.Bson.ObjectId?)null
                : request.WarehouseId.ToObjectId();

            var stocks = await stockRepo.GetLowStockItemsAsync(warehouseId, cancellationToken);

            var dtos = new List<StockDto>();

            foreach (var stock in stocks)
            {
                // Get product details
                var product = await productRepo.GetByIdAsync(stock.ProductId, cancellationToken);
                var warehouse = await warehouseRepo.GetByIdAsync(stock.WarehouseId, cancellationToken);

                dtos.Add(new StockDto
                {
                    Id = stock.Id.ToString(),
                    ProductId = stock.ProductId.ToString(),
                    ProductName = product?.Name.GetValueOrDefault("en", "Unknown"),
                    ProductSku = product?.Sku.Code,
                    VariantId = stock.VariantId?.ToString(),
                    WarehouseId = stock.WarehouseId.ToString(),
                    WarehouseName = warehouse?.Name,
                    QuantityOnHand = stock.StockLevel.QuantityOnHand,
                    ReservedQuantity = stock.StockLevel.ReservedQuantity,
                    AvailableQuantity = stock.StockLevel.AvailableQuantity,
                    ReorderLevel = stock.StockLevel.ReorderLevel,
                    MaximumLevel = stock.StockLevel.MaximumLevel,
                    ReorderQuantity = stock.StockLevel.ReorderQuantity,
                    IsLowStock = stock.StockLevel.IsLowStock,
                    LastCountedAt = stock.LastCountedAt,
                    LastMovementAt = stock.LastMovementAt,
                    CreatedAt = stock.CreatedAt,
                    UpdatedAt = stock.UpdatedAt
                });
            }

            return new BaseResponse<List<StockDto>>
            {
                Data = dtos
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting low stock items: {Error}", e.Message);
        }

        return new BaseResponse<List<StockDto>>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
