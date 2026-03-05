using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for getting all active leave types.
/// </summary>
public sealed class GetAllLeaveTypesQueryHandler(
    ILogger<GetAllLeaveTypesQueryHandler> logger,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetAllLeaveTypesQuery, BaseResponse<List<LeaveTypeDto>>>
{
    public async Task<BaseResponse<List<LeaveTypeDto>>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);

            var dtos = entities.Select(e => new LeaveTypeDto
            {
                Id = e.Id.ToString(),
                Name = e.Name,
                Description = e.Description,
                Code = e.Code,
                YearlyAllowance = e.YearlyAllowance,
                IsAccrualBased = e.IsAccrualBased,
                MonthlyAccrualDays = e.MonthlyAccrualDays,
                MaxCarryForwardDays = e.MaxCarryForwardDays,
                CarryForwardExpires = e.CarryForwardExpires,
                CarryForwardExpiryDate = e.CarryForwardExpiryDate,
                RequiresDocument = e.RequiresDocument,
                IsPaid = e.IsPaid,
                IsActive = e.IsActive,
                DisplayOrder = e.DisplayOrder,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToList();

            return new BaseResponse<List<LeaveTypeDto>>
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
            logger.LogError(e, "Error getting leave types: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveTypeDto>>
        {
            Data = [],
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
