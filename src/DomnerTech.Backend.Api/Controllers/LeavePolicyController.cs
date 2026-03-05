using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Features.LeavePolicies;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing leave policies.
/// </summary>
public sealed class LeavePolicyController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new leave policy.
    /// </summary>
    [HttpPost, Authorize(Roles = "LeavePolicy.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateLeavePolicy([FromBody] CreateLeavePolicyReqDto req)
    {
        var result = await commandQuery.Send(new CreateLeavePolicyCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing leave policy.
    /// </summary>
    [HttpPut, Authorize(Roles = "LeavePolicy.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateLeavePolicy([FromBody] UpdateLeavePolicyReqDto req)
    {
        var result = await commandQuery.Send(new UpdateLeavePolicyCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Deletes a leave policy.
    /// </summary>
    [HttpDelete("{id}"), Authorize(Roles = "LeavePolicy.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteLeavePolicy([FromRoute] string id)
    {
        var result = await commandQuery.Send(new DeleteLeavePolicyCommand(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves a leave policy by ID.
    /// </summary>
    [HttpGet("{id}"), Authorize(Roles = "LeavePolicy.Read")]
    public async Task<ActionResult<BaseResponse<LeavePolicyDto>>> GetLeavePolicyById([FromRoute] string id)
    {
        var result = await commandQuery.Send(new GetLeavePolicyByIdQuery(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves all active leave policies.
    /// </summary>
    [HttpGet, Authorize(Roles = "LeavePolicy.Read")]
    public async Task<ActionResult<BaseResponse<List<LeavePolicyDto>>>> GetActivePolicies()
    {
        var result = await commandQuery.Send(new GetActivePoliciesQuery(), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves the policy for a specific leave type.
    /// </summary>
    [HttpGet("leavetype/{leaveTypeId}"), Authorize(Roles = "LeavePolicy.Read")]
    public async Task<ActionResult<BaseResponse<LeavePolicyDto>>> GetPolicyByLeaveType([FromRoute] string leaveTypeId)
    {
        var result = await commandQuery.Send(new GetPolicyByLeaveTypeQuery(leaveTypeId), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
