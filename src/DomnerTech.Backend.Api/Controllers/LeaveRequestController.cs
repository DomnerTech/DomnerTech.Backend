using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;
using DomnerTech.Backend.Application.Features.LeaveRequests;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing leave requests.
/// </summary>
public sealed class LeaveRequestController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Write' role. Employees can create their own leave requests.</remarks>
    /// <param name="req">The leave request creation request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with the
    /// created leave request ID.</returns>
    [HttpPost, Authorize(Roles = "LeaveRequest.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateLeaveRequest([FromBody] CreateLeaveRequestReqDto req)
    {
        var result = await commandQuery.Send(new CreateLeaveRequestCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Write' role. Only pending requests can be updated.</remarks>
    /// <param name="req">The leave request update request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the update was successful.</returns>
    [HttpPut, Authorize(Roles = "LeaveRequest.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateLeaveRequest([FromBody] UpdateLeaveRequestReqDto req)
    {
        var result = await commandQuery.Send(new UpdateLeaveRequestCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Cancels a leave request.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Write' role. Employees can cancel their own pending or approved requests.</remarks>
    /// <param name="req">The cancellation request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the cancellation was successful.</returns>
    [HttpPost("cancel"), Authorize(Roles = "LeaveRequest.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> CancelLeaveRequest([FromBody] CancelLeaveRequestReqDto req)
    {
        var result = await commandQuery.Send(new CancelLeaveRequestCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves a leave request by ID.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Read' role.</remarks>
    /// <param name="id">The leave request ID.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a
    /// <see cref="LeaveRequestDetailDto"/> object.</returns>
    [HttpGet("{id}"), Authorize(Roles = "LeaveRequest.Read")]
    public async Task<ActionResult<BaseResponse<LeaveRequestDetailDto>>> GetLeaveRequestById([FromRoute] string id)
    {
        var result = await commandQuery.Send(new GetLeaveRequestByIdQuery(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves the current user's leave requests.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Read' role. Returns all leave requests for the authenticated user.</remarks>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list
    /// of <see cref="LeaveRequestDto"/> objects.</returns>
    [HttpGet("my"), Authorize(Roles = "LeaveRequest.Read")]
    public async Task<ActionResult<BaseResponse<List<LeaveRequestDto>>>> GetMyLeaveRequests()
    {
        var result = await commandQuery.Send(new GetMyLeaveRequestsQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves leave requests by status.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveRequest.Admin' role. Used by managers and HR to view requests needing attention.</remarks>
    /// <param name="status">The leave request status (Pending, Approved, Rejected, Cancelled).</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list
    /// of <see cref="LeaveRequestDto"/> objects.</returns>
    [HttpGet("status/{status}"), Authorize(Roles = "LeaveRequest.Admin")]
    public async Task<ActionResult<BaseResponse<List<LeaveRequestDto>>>> GetLeaveRequestsByStatus([FromRoute] string status)
    {
        var result = await commandQuery.Send(new GetLeaveRequestsByStatusQuery(status), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
