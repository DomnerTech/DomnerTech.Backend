using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;
using DomnerTech.Backend.Application.Features.Localizes;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class LocalizeController(
    CommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    [HttpPost("error-message/upsert"), Authorize(Roles = "Localize.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> ErrorMessageLocalizeUpsert(
        [FromBody] ErrorMessageLocalizeUpsertReqDto r)
    {
        var res = await commandQuery.Send(
            new ErrorMessageLocalizeUpsertCommand(r.Key, r.Messages),
            HttpContext.RequestAborted);
        return await ReturnJson(res);
    }
}