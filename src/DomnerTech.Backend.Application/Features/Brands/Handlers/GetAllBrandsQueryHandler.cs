using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Brands.Handlers;

/// <summary>
/// Handler for getting all brands.
/// </summary>
public sealed class GetAllBrandsQueryHandler(
    ILogger<GetAllBrandsQueryHandler> logger,
    IBrandRepo brandRepo) : IRequestHandler<GetAllBrandsQuery, BaseResponse<List<BrandDto>>>
{
    public async Task<BaseResponse<List<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var brands = await brandRepo.GetAllActiveAsync(cancellationToken);

            var dtos = brands.Select(b => new BrandDto
            {
                Id = b.Id.ToString(),
                Name = b.Name,
                Description = b.Description,
                Slug = b.Slug,
                LogoUrl = b.LogoUrl,
                WebsiteUrl = b.WebsiteUrl,
                IsActive = b.IsActive,
                DisplayOrder = b.DisplayOrder,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            }).ToList();

            return new BaseResponse<List<BrandDto>>
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
            logger.LogError(e, "Error getting brands: {Error}", e.Message);
        }

        return new BaseResponse<List<BrandDto>>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
