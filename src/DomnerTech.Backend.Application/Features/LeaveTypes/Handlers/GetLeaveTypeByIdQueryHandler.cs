using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for getting a leave type by ID.
/// </summary>
public sealed class GetLeaveTypeByIdQueryHandler(
    ILogger<GetLeaveTypeByIdQueryHandler> logger,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetLeaveTypeByIdQuery, BaseResponse<LeaveTypeDto>>
{
    public async Task<BaseResponse<LeaveTypeDto>> Handle(GetLeaveTypeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveTypeId = ObjectId.Parse(request.Id);
            var entity = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException("Leave type not found");
            }

            var dto = new LeaveTypeDto
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Description = entity.Description,
                Code = entity.Code,
                YearlyAllowance = entity.YearlyAllowance,
                IsAccrualBased = entity.IsAccrualBased,
                MonthlyAccrualDays = entity.MonthlyAccrualDays,
                MaxCarryForwardDays = entity.MaxCarryForwardDays,
                CarryForwardExpires = entity.CarryForwardExpires,
                CarryForwardExpiryDate = entity.CarryForwardExpiryDate,
                RequiresDocument = entity.RequiresDocument,
                IsPaid = entity.IsPaid,
                IsActive = entity.IsActive,
                DisplayOrder = entity.DisplayOrder,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return new BaseResponse<LeaveTypeDto>
            {
                Data = dto
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting leave type by ID: {Error}", e.Message);
        }

        return new BaseResponse<LeaveTypeDto>
        {
            Data = null!,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
