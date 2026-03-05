using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Employees;

namespace DomnerTech.Backend.Application.Features.Employees;

/// <summary>
/// Command for updating an existing employee's information.
/// </summary>
public sealed record UpdateEmployeeCommand(UpdateEmployeeReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
