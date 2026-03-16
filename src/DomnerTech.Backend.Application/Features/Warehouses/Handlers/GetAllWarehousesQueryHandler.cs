using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Warehouses.Handlers;

/// <summary>
/// Handler for getting all warehouses.
/// </summary>
public sealed class GetAllWarehousesQueryHandler(
    ILogger<GetAllWarehousesQueryHandler> logger,
    IWarehouseRepo warehouseRepo) : IRequestHandler<GetAllWarehousesQuery, BaseResponse<List<WarehouseDto>>>
{
    public async Task<BaseResponse<List<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouses = await warehouseRepo.GetAllActiveAsync(cancellationToken);

            var dtos = warehouses.Select(w => new WarehouseDto
            {
                Id = w.Id.ToString(),
                Name = w.Name,
                Code = w.Code,
                Type = w.Type,
                Address = w.Address,
                City = w.City,
                Country = w.Country,
                Phone = w.Phone,
                Email = w.Email,
                ManagerId = w.ManagerId?.ToString(),
                IsActive = w.IsActive,
                IsDefault = w.IsDefault,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            }).ToList();

            return new BaseResponse<List<WarehouseDto>>
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
            logger.LogError(e, "Error getting warehouses: {Error}", e.Message);
        }

        return new BaseResponse<List<WarehouseDto>>
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
