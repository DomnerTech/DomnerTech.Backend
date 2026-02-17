using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Auth;
using DomnerTech.Backend.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class AuthController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpPost("login"), AllowAnonymous]
    public async Task<ActionResult<BaseResponse<LoginResDto>>> Login([FromBody] LoginReqDto req)
    {
        var res = await commandQuery.Send(new LoginCommand(req.Username, req.Pwd), HttpContext.RequestAborted);
        return res.ReturnJson();
    }
}