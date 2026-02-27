using DomnerTech.Backend.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class BaseApiController : ControllerBase
{
    protected string UserReqId => User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId)?.Value ?? string.Empty;
    protected string CompanyId => User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.CompanyId)?.Value ?? string.Empty;
}