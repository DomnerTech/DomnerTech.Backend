using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Roles;
using DomnerTech.Backend.Application.Features.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class RoleController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpPost, Authorize(Roles = "Role.Write")]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleReqDto req)
    {
        var res = await commandQuery.Send(
            new CreateRoleCommand(req.Name, req.Desc),
            HttpContext.RequestAborted);
        return res.ReturnJson();
    }

    [HttpGet, Authorize("Role.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<RoleDto>>>> GetAllRoles()
    {
        var res = await commandQuery.Send(new GetAllRolesQuery(), HttpContext.RequestAborted);
        return res.ReturnJson();
    }

    [HttpGet("user-roles/{userId}"), Authorize(Roles = "Role.Read")]
    public async Task<ActionResult> GetUserRoles(string userId)
    {
        var res = await commandQuery.Send(
            new GetUserRolesQuery(userId),
            HttpContext.RequestAborted);
        return res.ReturnJson();
    }
}