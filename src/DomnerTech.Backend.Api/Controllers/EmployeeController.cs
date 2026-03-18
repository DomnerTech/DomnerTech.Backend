using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Employees;
using DomnerTech.Backend.Application.Features.Employees;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class EmployeeController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    [HttpPost, Authorize(Roles = "Employee.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> CreateEmployee([FromBody] CreateEmployeeReqDto req)
    {
        var result = await _commandQuery.Send(new CreateEmployeeCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    /// <remarks>This endpoint requires the 'Employee.Write' role. All employee fields can be updated except
    /// the employee number and hire date which are immutable after creation.</remarks>
    /// <param name="req">The employee update request containing the employee ID and updated information.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the update was successful.</returns>
    [HttpPut, Authorize(Roles = "Employee.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateEmployee([FromBody] UpdateEmployeeReqDto req)
    {
        var result = await _commandQuery.Send(new UpdateEmployeeCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves a paginated list of employees, supporting cursor-based navigation and sorting options.
    /// </summary>
    /// <remarks>This endpoint requires the 'Employee.Read' role. Cursor-based pagination enables efficient
    /// navigation through large result sets. Sorting and direction parameters allow flexible result ordering.</remarks>
    /// <param name="cursor">The cursor representing the position in the result set from which to continue retrieving employees. Can be
    /// null to start from the beginning.</param>
    /// <param name="pageSize">The maximum number of employees to return in the response. Must be a positive integer.</param>
    /// <param name="direction">The direction in which to paginate the results. Specify forward or backward to navigate through the result set.</param>
    /// <param name="sortBy">The field by which to sort the employees. Must correspond to a valid sortable property.</param>
    /// <param name="includeTotalCount">Specifies whether to include the total count of employees in the response. Set to <see langword="true"/> to
    /// include the count; otherwise, <see langword="false"/>.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a <see
    /// cref="KeysetPageResult{T}"/> of <see cref="EmployeeDto"/> objects. The result includes the requested
    /// page of employees and, if specified, the total count.</returns>
    [HttpGet, Authorize(Roles = "Employee.Read")]
    public async Task<ActionResult<BaseResponse<KeysetPageResult<EmployeeDto>>>> GetEmployees(
        [FromQuery(Name = "cursor")] string? cursor,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "direction")] CursorDirection direction,
        [FromQuery(Name = "sort_by")] string sortBy,
        [FromQuery(Name = "include_total_count")] bool includeTotalCount)
    {
        var res = await _commandQuery.Send(new GetEmployeePageQuery
        {
            Cursor = cursor,
            Direction = direction,
            IncludeTotalCount = includeTotalCount,
            PageSize = pageSize,
            SortKey = sortBy
        }, HttpContext.RequestAborted);
        return await ReturnJson(res);
    }
}