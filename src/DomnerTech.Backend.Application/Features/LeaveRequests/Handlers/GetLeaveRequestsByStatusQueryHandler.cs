using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class GetLeaveRequestsByStatusQueryHandler(
    ILogger<GetLeaveRequestsByStatusQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<GetLeaveRequestsByStatusQuery, BaseResponse<IEnumerable<LeaveRequestDto>>>
{
    public async Task<BaseResponse<IEnumerable<LeaveRequestDto>>> Handle(GetLeaveRequestsByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<LeaveRequestStatus>(request.Status, true, out var status))
            {
                throw new ValidationException("Invalid leave request status");
            }

            var entities = await leaveRequestRepo.GetByStatusAsync(status, cancellationToken);
            return new BaseResponse<IEnumerable<LeaveRequestDto>>
            {
                Data = entities.Select(e => e.ToDto())
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting leave requests by status: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<LeaveRequestDto>>
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
