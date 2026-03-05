using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Employees;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Pagination;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.Features.Employees.Handlers;

/// <summary>
/// Handler for retrieving paginated employee lists.
/// </summary>
public sealed class GetEmployeePageQueryHandler(
    IKeysetPaginator<EmployeeEntity> paginator,
    ITenantService tenantService) : IRequestHandler<GetEmployeePageQuery, BaseResponse<KeysetPageResult<EmployeeDto>>>
{
    /// <summary>
    /// Handles the employee page query request.
    /// </summary>
    /// <param name="request">The employee page query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated result of employee DTOs.</returns>
    public async Task<BaseResponse<KeysetPageResult<EmployeeDto>>> Handle(GetEmployeePageQuery request, CancellationToken cancellationToken)
    {
        var tenantId = tenantService.CompanyId.ToObjectId();
        var paging = await paginator.PaginateAsync(
            DatabaseNameConstant.DatabaseName,
            tenantId,
            request,
            ct: cancellationToken);
        return new BaseResponse<KeysetPageResult<EmployeeDto>>
        {
            Data = paging.ToDto(i => i.ToDto())
        };
    }
}
