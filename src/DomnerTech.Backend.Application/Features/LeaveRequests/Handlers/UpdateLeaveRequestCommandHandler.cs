using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class UpdateLeaveRequestCommandHandler(
    ILogger<UpdateLeaveRequestCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveValidationService leaveValidationService) : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var requestId = ObjectId.Parse(r.Id);

            var existing = await leaveRequestRepo.GetByIdAsync(requestId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave request not found");
            }

            if (existing.Status != LeaveRequestStatus.Pending)
            {
                throw new ValidationException("Only pending leave requests can be updated");
            }

            var isHalfDay = r.RequestType != LeaveRequestType.FullDay;
            var totalDays = await leaveValidationService.CalculateLeaveDaysAsync(
                existing.LeaveTypeId, r.StartDate, r.EndDate, isHalfDay, cancellationToken);

            var (isValid, errors) = await leaveValidationService.ValidateLeaveRequestAsync(
                existing.EmployeeId, existing.LeaveTypeId, r.StartDate, r.EndDate, totalDays, requestId, cancellationToken);

            if (!isValid)
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            existing.Period.StartDate = r.StartDate.Date;
            existing.Period.EndDate = r.EndDate.Date;
            existing.RequestType = r.RequestType;
            existing.TotalDays = totalDays;
            existing.Reason = r.Reason;
            existing.Notes = r.Notes;
            existing.DocumentUrls = r.DocumentUrls;
            existing.UpdatedAt = DateTime.UtcNow;

            await leaveRequestRepo.UpdateAsync(existing, cancellationToken);

            return new BaseResponse<bool> { Data = true };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating leave request: {Error}", e.Message);
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