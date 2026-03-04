using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Employees;
using DomnerTech.Backend.Application.Features.Employees;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class EmployeeController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    [HttpPost, Authorize(Roles = "Employee.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> CreateEmployee([FromBody] CreateEmployeeReqDto req)
    {
        var result = await commandQuery.Send(new CreateEmployeeCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}