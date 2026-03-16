using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for getting product by ID.
/// </summary>
public sealed class GetProductByIdQueryHandler(
    ILogger<GetProductByIdQueryHandler> logger,
    IProductRepo productRepo,
    ICategoryRepo categoryRepo,
    IBrandRepo brandRepo) : IRequestHandler<GetProductByIdQuery, BaseResponse<ProductDto>>
{
    public async Task<BaseResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productId = request.ProductId.ToObjectId();
            var product = await productRepo.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return new BaseResponse<ProductDto>
                {
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Product not found"
                    }
                };
            }

            // Get category name
            var category = await categoryRepo.GetByIdAsync(product.CategoryId, cancellationToken);
            var categoryName = category?.Name.GetValueOrDefault("en", "Unknown");

            // Get brand name
            string? brandName = null;
            if (product.BrandId.HasValue)
            {
                var brand = await brandRepo.GetByIdAsync(product.BrandId.Value, cancellationToken);
                brandName = brand?.Name;
            }

            var dto = new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Sku = product.Sku.Code,
                Barcodes = product.Barcodes?.Select(b => new BarcodeDto
                {
                    Value = b.Value,
                    Type = b.Type
                }).ToList(),
                CategoryId = product.CategoryId.ToString(),
                CategoryName = categoryName,
                BrandId = product.BrandId?.ToString(),
                BrandName = brandName,
                Prices = product.Prices.Select(p => new ProductPriceDto
                {
                    PriceType = p.PriceType,
                    Currency = p.Currency,
                    Amount = p.Amount,
                    EffectiveFrom = p.EffectiveFrom,
                    EffectiveTo = p.EffectiveTo
                }).ToList(),
                CostPrice = product.CostPrice,
                UnitOfMeasure = product.UnitOfMeasure,
                Attributes = product.Attributes?.Select(a => new ProductAttributeDto
                {
                    Name = a.Name,
                    Value = a.Value
                }).ToList(),
                Images = product.Images,
                HasVariants = product.HasVariants,
                IsBundle = product.IsBundle,
                Status = product.Status,
                TrackInventory = product.TrackInventory,
                TrackBatchLot = product.TrackBatchLot,
                TrackSerialNumber = product.TrackSerialNumber,
                IsTaxable = product.IsTaxable,
                TaxRate = product.TaxRate,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Tags = product.Tags,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return new BaseResponse<ProductDto>
            {
                Data = dto
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting product: {Error}", e.Message);
        }

        return new BaseResponse<ProductDto>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
