using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class UserController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpGet("get-me")]
    public async Task<ActionResult<BaseResponse<UserDto>>> GetMe()
    {
        var user = await commandQuery.Send(new GetUserQuery(UserReqId), HttpContext.RequestAborted);
        return user.ReturnJson();
    }

    [HttpPost, Authorize(Roles = "User.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> CreateUser([FromBody] CreateUserDto r)
    {
        var result = await commandQuery.Send(new CreateUserCommand(r.Username, r.Pwd), HttpContext.RequestAborted);
        return result.ReturnJson();
    }

    [HttpGet("all"), Authorize(Roles = "User.Read")]
    public async Task<ActionResult<BaseResponse<bool>>> GetAllUser()
    {
        var result = await commandQuery.Send(new GetAllUsersQuery(), HttpContext.RequestAborted);
        return result.ReturnJson();
    }
}