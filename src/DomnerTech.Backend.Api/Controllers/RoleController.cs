using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs.Roles;
using DomnerTech.Backend.Application.Features.Roles;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class RoleController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleReqDto req)
    {
        var res = await commandQuery.Send(
            new CreateRoleCommand(req.Name, req.Desc),
            HttpContext.RequestAborted);
        return res.ReturnJson();
    }
}