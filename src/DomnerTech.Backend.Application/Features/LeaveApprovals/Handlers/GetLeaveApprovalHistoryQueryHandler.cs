using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Handlers;

public sealed class GetLeaveApprovalHistoryQueryHandler(
    ILogger<GetLeaveApprovalHistoryQueryHandler> logger,
    ILeaveApprovalRepo leaveApprovalRepo) : IRequestHandler<GetLeaveApprovalHistoryQuery, BaseResponse<IEnumerable<LeaveApprovalDto>>>
{
    public async Task<BaseResponse<IEnumerable<LeaveApprovalDto>>> Handle(GetLeaveApprovalHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveRequestId = ObjectId.Parse(request.LeaveRequestId);
            var entities = await leaveApprovalRepo.GetByLeaveRequestAsync(leaveRequestId, cancellationToken);
            return new BaseResponse<IEnumerable<LeaveApprovalDto>>
            {
                Data = entities.Select(e => e.ToDto())
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting approval history: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<LeaveApprovalDto>>
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
