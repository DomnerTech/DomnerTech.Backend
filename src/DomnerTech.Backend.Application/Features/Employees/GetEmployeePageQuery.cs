using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Employees;
using DomnerTech.Backend.Application.Pagination;

namespace DomnerTech.Backend.Application.Features.Employees;

/// <summary>
/// Query to retrieve a paginated list of employees using keyset pagination.
/// </summary>
public sealed class GetEmployeePageQuery :
    KeysetPageRequest,
    IRequest<BaseResponse<KeysetPageResult<EmployeeDto>>>,
    ILogReqCreator,
    IValidatableRequest;
