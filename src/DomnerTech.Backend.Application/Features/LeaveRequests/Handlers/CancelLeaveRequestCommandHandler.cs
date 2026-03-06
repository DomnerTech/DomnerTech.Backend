using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class CancelLeaveRequestCommandHandler(
    ILogger<CancelLeaveRequestCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<CancelLeaveRequestCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
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

            if (existing.Status == LeaveRequestStatus.Cancelled)
            {
                throw new ValidationException("Leave request is already cancelled");
            }

            if (existing.Status == LeaveRequestStatus.Rejected)
            {
                throw new ValidationException("Cannot cancel a rejected leave request");
            }

            existing.Status = LeaveRequestStatus.Cancelled;
            existing.CancellationReason = r.CancellationReason;
            existing.CancelledAt = DateTime.UtcNow;
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
            logger.LogError(e, "Error cancelling leave request: {Error}", e.Message);
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