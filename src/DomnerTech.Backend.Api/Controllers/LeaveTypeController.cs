using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;
using DomnerTech.Backend.Application.Features.LeaveTypes;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing leave types.
/// </summary>
public sealed class LeaveTypeController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new leave type.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveType.Write' role. Only administrators or HR personnel
    /// can create leave types.</remarks>
    /// <param name="req">The leave type creation request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with the
    /// created leave type ID.</returns>
    [HttpPost, Authorize(Roles = "LeaveType.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateLeaveType([FromBody] CreateLeaveTypeReqDto req)
    {
        var result = await commandQuery.Send(new CreateLeaveTypeCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing leave type.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveType.Write' role. Only administrators or HR personnel
    /// can update leave types.</remarks>
    /// <param name="req">The leave type update request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the update was successful.</returns>
    [HttpPut, Authorize(Roles = "LeaveType.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateLeaveType([FromBody] UpdateLeaveTypeReqDto req)
    {
        var result = await commandQuery.Send(new UpdateLeaveTypeCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Deletes a leave type.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveType.Write' role. Deletion is a soft delete operation.</remarks>
    /// <param name="id">The leave type ID to delete.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}"), Authorize(Roles = "LeaveType.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteLeaveType([FromRoute] string id)
    {
        var result = await commandQuery.Send(new DeleteLeaveTypeCommand(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves all active leave types.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveType.Read' role. Returns all active leave types
    /// ordered by display order and name.</remarks>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list
    /// of <see cref="LeaveTypeDto"/> objects.</returns>
    [HttpGet, Authorize(Roles = "LeaveType.Read")]
    public async Task<ActionResult<BaseResponse<List<LeaveTypeDto>>>> GetAllLeaveTypes()
    {
        var result = await commandQuery.Send(new GetAllLeaveTypesQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves a leave type by ID.
    /// </summary>
    /// <remarks>This endpoint requires the 'LeaveType.Read' role.</remarks>
    /// <param name="id">The leave type ID.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a
    /// <see cref="LeaveTypeDto"/> object.</returns>
    [HttpGet("{id}"), Authorize(Roles = "LeaveType.Read")]
    public async Task<ActionResult<BaseResponse<LeaveTypeDto>>> GetLeaveTypeById([FromRoute] string id)
    {
        var result = await commandQuery.Send(new GetLeaveTypeByIdQuery(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
