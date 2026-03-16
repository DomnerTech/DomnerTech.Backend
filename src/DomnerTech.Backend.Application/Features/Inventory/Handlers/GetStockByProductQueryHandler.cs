using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Inventory.Handlers;

/// <summary>
/// Handler for getting stock by product.
/// </summary>
public sealed class GetStockByProductQueryHandler(
    ILogger<GetStockByProductQueryHandler> logger,
    IStockRepo stockRepo,
    IProductRepo productRepo,
    IWarehouseRepo warehouseRepo) : IRequestHandler<GetStockByProductQuery, BaseResponse<List<StockDto>>>
{
    public async Task<BaseResponse<List<StockDto>>> Handle(GetStockByProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productId = request.ProductId.ToObjectId();
            var warehouseId = string.IsNullOrWhiteSpace(request.WarehouseId)
                ? (MongoDB.Bson.ObjectId?)null
                : request.WarehouseId.ToObjectId();
            var variantId = string.IsNullOrWhiteSpace(request.VariantId)
                ? (MongoDB.Bson.ObjectId?)null
                : request.VariantId.ToObjectId();

            // Get product to verify it exists
            var product = await productRepo.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                return new BaseResponse<List<StockDto>>
                {
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Product not found"
                    }
                };
            }

            // Get stocks
            List<StockEntity> stocks = [];

            if (warehouseId.HasValue)
            {
                var s = await stockRepo.GetByProductAndWarehouseAsync(productId, warehouseId.Value, variantId, cancellationToken);
                if(s != null)
                {
                    stocks.Add(s);
                }
            }

            if (stocks.Count == 0)
            {
                stocks = await stockRepo.GetByProductIdAsync(productId, cancellationToken);
            }

            var dtos = new List<StockDto>();

            foreach (var stock in stocks)
            {
                var warehouse = await warehouseRepo.GetByIdAsync(stock.WarehouseId, cancellationToken);

                dtos.Add(new StockDto
                {
                    Id = stock.Id.ToString(),
                    ProductId = stock.ProductId.ToString(),
                    ProductName = product.Name.GetValueOrDefault("en", "Unknown"),
                    ProductSku = product.Sku.Code,
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
            logger.LogError(e, "Error getting stock by product: {Error}", e.Message);
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
