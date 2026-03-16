using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Categories.Handlers;

/// <summary>
/// Handler for creating a new category.
/// </summary>
public sealed class CreateCategoryCommandHandler(
    ILogger<CreateCategoryCommandHandler> logger,
    ICategoryRepo categoryRepo,
    ITenantService tenantService) : IRequestHandler<CreateCategoryCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;

            // Check if slug already exists
            var existingCategory = await categoryRepo.GetBySlugAsync(dto.Slug, cancellationToken);
            if (existingCategory != null)
            {
                return new BaseResponse<string>
                {
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Category with this slug already exists"
                    }
                };
            }

            // Validate parent category if provided
            if (!string.IsNullOrWhiteSpace(dto.ParentCategoryId))
            {
                var parentId = dto.ParentCategoryId.ToObjectId();
                var parentCategory = await categoryRepo.GetByIdAsync(parentId, cancellationToken);
                if (parentCategory == null)
                {
                    return new BaseResponse<string>
                    {
                        Status = new ResponseStatus
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            ErrorCode = ErrorCodes.SystemError,
                            Desc = "Parent category not found"
                        }
                    };
                }
            }

            var now = DateTime.UtcNow;
            var categoryId = ObjectId.GenerateNewId();

            var entity = new CategoryEntity
            {
                Id = categoryId,
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Name = dto.Name,
                Description = dto.Description,
                Slug = dto.Slug,
                ParentCategoryId = dto.ParentCategoryId?.ToObjectId(),
                ImageUrl = dto.ImageUrl,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            };

            await categoryRepo.CreateAsync(entity, cancellationToken);

            logger.LogInformation("Category created successfully: {CategoryId} - {Slug}", categoryId, dto.Slug);

            return new BaseResponse<string>
            {
                Data = categoryId.ToString()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating category: {Error}", e.Message);
        }

        return new BaseResponse<string>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
