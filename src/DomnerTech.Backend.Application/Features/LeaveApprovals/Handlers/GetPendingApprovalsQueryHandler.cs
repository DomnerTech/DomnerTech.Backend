using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Handlers;

public sealed class GetPendingApprovalsQueryHandler(
    ILogger<GetPendingApprovalsQueryHandler> logger,
    ILeaveApprovalRepo leaveApprovalRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetPendingApprovalsQuery, BaseResponse<List<LeaveApprovalDto>>>
{
    public async Task<BaseResponse<List<LeaveApprovalDto>>> Handle(GetPendingApprovalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var approverId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            if (approverId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            var entities = await leaveApprovalRepo.GetPendingByApproverAsync(approverId, cancellationToken);
            return new BaseResponse<List<LeaveApprovalDto>>
            {
                Data = [.. entities.Select(e => e.ToDto())]
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
            logger.LogError(e, "Error getting pending approvals: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveApprovalDto>>
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