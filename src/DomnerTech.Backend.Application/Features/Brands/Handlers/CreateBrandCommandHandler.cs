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

namespace DomnerTech.Backend.Application.Features.Brands.Handlers;

/// <summary>
/// Handler for creating a new brand.
/// </summary>
public sealed class CreateBrandCommandHandler(
    ILogger<CreateBrandCommandHandler> logger,
    IBrandRepo brandRepo,
    ITenantService tenantService) : IRequestHandler<CreateBrandCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;

            var now = DateTime.UtcNow;
            var brandId = ObjectId.GenerateNewId();

            var entity = new BrandEntity
            {
                Id = brandId,
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Name = dto.Name,
                Description = dto.Description,
                LogoUrl = dto.LogoUrl,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            await brandRepo.CreateAsync(entity, cancellationToken);

            return new BaseResponse<string>
            {
                Data = brandId.ToString()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating brand: {Error}", e.Message);
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
