using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs.Roles;
using DomnerTech.Backend.Application.Features.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class RoleController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpPost, Authorize(Roles = "RoleWrite")]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleReqDto req)
    {
        var res = await commandQuery.Send(
            new CreateRoleCommand(req.Name, req.Desc),
            HttpContext.RequestAborted);
        return res.ReturnJson();
    }
    [HttpGet, Authorize(Roles = "RoleRead")]
    public async Task<ActionResult> GetUserRoles([FromBody] CreateRoleReqDto req)
    {
        var res = await commandQuery.Send(
            new CreateRoleCommand(req.Name, req.Desc),
            HttpContext.RequestAborted);
        return res.ReturnJson();
    }
}