using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class GetMyLeaveRequestsQueryHandler(
    ILogger<GetMyLeaveRequestsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyLeaveRequestsQuery, BaseResponse<IEnumerable<LeaveRequestDto>>>
{
    public async Task<BaseResponse<IEnumerable<LeaveRequestDto>>> Handle(GetMyLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId);

            if (string.IsNullOrEmpty(employeeId) || !ObjectId.TryParse(employeeId, out var empId))
            {
                throw new UnauthorizedException();
            }

            var entities = await leaveRequestRepo.GetByEmployeeAsync(empId, cancellationToken);
            return new BaseResponse<IEnumerable<LeaveRequestDto>>
            {
                Data = entities.Select(e => e.ToDto())
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (UnauthorizedException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting my leave requests: {Error}", e.Message);
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