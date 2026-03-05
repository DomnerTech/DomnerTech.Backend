using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for updating a leave type.
/// </summary>
public sealed class UpdateLeaveTypeCommandHandler(
    ILogger<UpdateLeaveTypeCommandHandler> logger,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<UpdateLeaveTypeCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var leaveTypeId = ObjectId.Parse(r.Id);

            var existing = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave type not found");
            }

            existing.Name = r.Name;
            existing.Description = r.Description;
            existing.YearlyAllowance = r.YearlyAllowance;
            existing.IsAccrualBased = r.IsAccrualBased;
            existing.MonthlyAccrualDays = r.MonthlyAccrualDays;
            existing.MaxCarryForwardDays = r.MaxCarryForwardDays;
            existing.CarryForwardExpires = r.CarryForwardExpires;
            existing.CarryForwardExpiryDate = r.CarryForwardExpiryDate;
            existing.RequiresDocument = r.RequiresDocument;
            existing.IsPaid = r.IsPaid;
            existing.IsActive = r.IsActive;
            existing.DisplayOrder = r.DisplayOrder;
            existing.UpdatedAt = DateTime.UtcNow;

            await leaveTypeRepo.UpdateAsync(existing, cancellationToken);

            return new BaseResponse<bool>
            {
                Data = true
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
            logger.LogError(e, "Error updating leave type: {Error}", e.Message);
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
