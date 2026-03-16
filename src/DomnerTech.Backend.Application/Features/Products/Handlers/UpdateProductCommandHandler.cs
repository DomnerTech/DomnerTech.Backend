using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for updating an existing product.
/// </summary>
public sealed class UpdateProductCommandHandler(
    ILogger<UpdateProductCommandHandler> logger,
    IProductRepo productRepo,
    ICategoryRepo categoryRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateProductCommand, BaseResponse<bool>>
{
    private HttpContext HttpContext => httpContextAccessor.HttpContext ?? throw new Exception("HttpContext is null");

    public async Task<BaseResponse<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productId = request.ProductId.ToObjectId();
            var dto = request.Dto;

            // Get existing product
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

            // Validate category exists
            var categoryId = dto.CategoryId.ToObjectId();
            var category = await categoryRepo.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Category not found"
                    }
                };
            }

            var now = DateTime.UtcNow;

            // Update properties
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = categoryId;
            product.BrandId = dto.BrandId?.ToObjectId();
            product.Prices = [.. dto.Prices.Select(p => new ProductPriceValueObject
            {
                PriceType = p.PriceType,
                Currency = p.Currency,
                Amount = p.Amount,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo
            })];
            product.CostPrice = dto.CostPrice;
            product.UnitOfMeasure = dto.UnitOfMeasure;
            product.Attributes = dto.Attributes?.Select(a => new ProductAttributeValueObject
            {
                Name = a.Name,
                Value = a.Value
            }).ToList();
            product.Images = dto.Images;
            product.TrackInventory = dto.TrackInventory;
            product.TrackBatchLot = dto.TrackBatchLot;
            product.TrackSerialNumber = dto.TrackSerialNumber;
            product.IsTaxable = dto.IsTaxable;
            product.TaxRate = dto.TaxRate;
            product.Weight = dto.Weight;
            product.Dimensions = dto.Dimensions;
            product.Tags = dto.Tags;
            product.WarehouseIds = dto.WarehouseIds?.Select(w => w.ToObjectId()).ToList();
            product.UpdatedAt = now;
            product.UpdatedBy = HttpContext.GetUserId().ToObjectId();

            await productRepo.UpdateAsync(product, cancellationToken);

            logger.LogInformation("Product updated successfully: {ProductId}", productId);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating product: {Error}", e.Message);
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
