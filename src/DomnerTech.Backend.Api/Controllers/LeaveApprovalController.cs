using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;
using DomnerTech.Backend.Application.Features.LeaveApprovals;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing leave approvals.
/// </summary>
public sealed class LeaveApprovalController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Approves a leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveApproval.Write' role. Managers and HR can approve leave requests assigned to them.</remarks>
    /// <param name="req">The approval request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean indicating success.</returns>
    [HttpPost("approve"), Authorize(Roles = "LeaveApproval.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> ApproveLeave([FromBody] ApproveLeaveReqDto req)
    {
        var result = await commandQuery.Send(new ApproveLeaveCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Rejects a leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveApproval.Write' role. Managers and HR can reject leave requests assigned to them.</remarks>
    /// <param name="req">The rejection request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean indicating success.</returns>
    [HttpPost("reject"), Authorize(Roles = "LeaveApproval.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> RejectLeave([FromBody] RejectLeaveReqDto req)
    {
        var result = await commandQuery.Send(new RejectLeaveCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves pending approval requests for the current user.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveApproval.Write' role. Returns all leave requests waiting for the user's approval.</remarks>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list of <see cref="LeaveApprovalDto"/> objects.</returns>
    [HttpGet("pending"), Authorize(Roles = "LeaveApproval.Write")]
    public async Task<ActionResult<BaseResponse<List<LeaveApprovalDto>>>> GetPendingApprovals()
    {
        var result = await commandQuery.Send(new GetPendingApprovalsQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves the approval history for a specific leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveApproval.Write' role. Shows all approval steps and their status for a leave request.</remarks>
    /// <param name="leaveRequestId">The leave request ID.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list of <see cref="LeaveApprovalDto"/> objects.</returns>
    [HttpGet("history/{leaveRequestId}"), Authorize(Roles = "LeaveApproval.Write")]
    public async Task<ActionResult<BaseResponse<List<LeaveApprovalDto>>>> GetApprovalHistory([FromRoute] string leaveRequestId)
    {
        var result = await commandQuery.Send(new GetLeaveApprovalHistoryQuery(leaveRequestId), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
