using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class UserController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpPost("get-user"), AllowAnonymous]
    public async Task<ActionResult<BaseResponse<UserDto>>> GetUser([FromBody] GetUserReqDto r)
    {
        var user = await commandQuery.Send(new GetUserQuery(r.UserId), HttpContext.RequestAborted);
        return user.ReturnJson();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<BaseResponse<bool>>> CreateUser([FromBody] CreateUserDto r)
    {
        var result = await commandQuery.Send(new CreateUserCommand(r.Username, r.Pwd), HttpContext.RequestAborted);
        return result.ReturnJson();
    }
}