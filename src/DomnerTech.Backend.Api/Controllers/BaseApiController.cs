using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Features.Localizes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class BaseApiController(ICommandQuery commandQuery) : ControllerBase
{
    protected ICommandQuery _commandQuery = commandQuery;
    protected string UserReqId => User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId)?.Value ?? string.Empty;
    protected string CompanyId => User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.CompanyId)?.Value ?? string.Empty;
    protected async Task<JsonResult> ReturnJson<T>(BaseResponse<T> obj)
    {
        if (!obj.IsSuccess)
        {
            obj.Status.Desc = await _commandQuery.Send(new ResolveErrorLocalizeCommand(obj.Status.ErrorCode, HttpContext.GetCurrentLanguage()));
        }

        return new JsonResult(obj)
        {
            StatusCode = obj.Status.StatusCode
        };
    }
}