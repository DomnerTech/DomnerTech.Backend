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

namespace DomnerTech.Backend.Application.Features.Warehouses.Handlers;

/// <summary>
/// Handler for creating a new warehouse.
/// </summary>
public sealed class CreateWarehouseCommandHandler(
    ILogger<CreateWarehouseCommandHandler> logger,
    IWarehouseRepo warehouseRepo,
    ITenantService tenantService) : IRequestHandler<CreateWarehouseCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;

            // Check if code already exists
            var existingWarehouse = await warehouseRepo.GetByCodeAsync(dto.Code, cancellationToken);
            if (existingWarehouse != null)
            {
                return new BaseResponse<string>
                {
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Warehouse with this code already exists"
                    }
                };
            }

            var now = DateTime.UtcNow;
            var warehouseId = ObjectId.GenerateNewId();

            var entity = new WarehouseEntity
            {
                Id = warehouseId,
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Name = dto.Name,
                Code = dto.Code,
                Type = dto.Type,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                ManagerId = dto.ManagerId?.ToObjectId(),
                IsActive = true,
                IsDefault = dto.IsDefault,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            };

            await warehouseRepo.CreateAsync(entity, cancellationToken);

            logger.LogInformation("Warehouse created successfully: {WarehouseId} - {Code}", warehouseId, dto.Code);

            return new BaseResponse<string>
            {
                Data = warehouseId.ToString()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating warehouse: {Error}", e.Message);
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
