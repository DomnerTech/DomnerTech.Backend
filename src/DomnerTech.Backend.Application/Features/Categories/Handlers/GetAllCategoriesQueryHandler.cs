using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Categories.Handlers;

/// <summary>
/// Handler for getting all categories.
/// </summary>
public sealed class GetAllCategoriesQueryHandler(
    ILogger<GetAllCategoriesQueryHandler> logger,
    ICategoryRepo categoryRepo) : IRequestHandler<GetAllCategoriesQuery, BaseResponse<List<CategoryDto>>>
{
    public async Task<BaseResponse<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var categories = request.ActiveOnly
                ? await categoryRepo.GetAllActiveAsync(cancellationToken)
                : await categoryRepo.GetAllActiveAsync(cancellationToken); // TODO: Add GetAllAsync method

            var dtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Description = c.Description,
                Slug = c.Slug,
                ParentCategoryId = c.ParentCategoryId?.ToString(),
                ImageUrl = c.ImageUrl,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return new BaseResponse<List<CategoryDto>>
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
            logger.LogError(e, "Error getting categories: {Error}", e.Message);
        }

        return new BaseResponse<List<CategoryDto>>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
