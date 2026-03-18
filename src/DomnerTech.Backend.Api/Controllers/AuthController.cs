using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Auth;
using DomnerTech.Backend.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class AuthController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    [HttpPost("login"), AllowAnonymous]
    public async Task<ActionResult<BaseResponse<LoginResDto>>> Login([FromBody] LoginReqDto req)
    {
        var res = await _commandQuery.Send(
            new LoginCommand(req.Username, req.Pwd),
            HttpContext.RequestAborted);

        if (res.Status.StatusCode == StatusCodes.Status200OK)
            Response.SetCookie(AuthConstant.TokenName, res.Data.Token);
        
        return await ReturnJson(res);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<BaseResponse<bool>>> Logout()
    {
        Response.RemoveCookie(AuthConstant.TokenName);
        return await ReturnJson(new BaseResponse<bool>());
    }
}