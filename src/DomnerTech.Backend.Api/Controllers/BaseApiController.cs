using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
//[Authorize]
[AllowAnonymous]
public class BaseApiController : ControllerBase;