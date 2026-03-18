using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;
using DomnerTech.Backend.Application.Features.Holidays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing holidays.
/// </summary>
public sealed class HolidayController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Creates a new holiday.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Write' role. Only administrators can create holidays.</remarks>
    /// <param name="req">The holiday creation request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with the
    /// created holiday ID.</returns>
    [HttpPost, Authorize(Roles = "Holiday.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateHoliday([FromBody] CreateHolidayReqDto req)
    {
        var result = await _commandQuery.Send(new CreateHolidayCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Bulk creates multiple holidays.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Write' role. Useful for importing annual holiday calendars.</remarks>
    /// <param name="req">The bulk creation request containing multiple holidays.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with the
    /// number of holidays created.</returns>
    [HttpPost("bulk"), Authorize(Roles = "Holiday.Write")]
    public async Task<ActionResult<BaseResponse<int>>> BulkCreateHolidays([FromBody] BulkCreateHolidaysReqDto req)
    {
        var result = await _commandQuery.Send(new BulkCreateHolidaysCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing holiday.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Write' role.</remarks>
    /// <param name="req">The holiday update request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the update was successful.</returns>
    [HttpPut, Authorize(Roles = "Holiday.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateHoliday([FromBody] UpdateHolidayReqDto req)
    {
        var result = await _commandQuery.Send(new UpdateHolidayCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Deletes a holiday.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Write' role. Deletion is a soft delete operation.</remarks>
    /// <param name="id">The holiday ID to delete.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a boolean
    /// indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}"), Authorize(Roles = "Holiday.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteHoliday([FromRoute] string id)
    {
        var result = await _commandQuery.Send(new DeleteHolidayCommand(id), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves holidays for a specific year.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Read' role. Returns all holidays in the specified year
    /// ordered by date.</remarks>
    /// <param name="year">The year to retrieve holidays for.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list
    /// of <see cref="HolidayDto"/> objects.</returns>
    [HttpGet("year/{year}"), Authorize(Roles = "Holiday.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<HolidayDto>>>> GetHolidaysByYear([FromRoute] int year)
    {
        var result = await _commandQuery.Send(new GetHolidaysByYearQuery(year), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Retrieves upcoming holidays.
    /// </summary>
    /// <remarks>This endpoint requires the 'Holiday.Read' role. Returns upcoming holidays from today onwards.</remarks>
    /// <param name="count">The maximum number of holidays to return (default: 10).</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a list
    /// of upcoming <see cref="HolidayDto"/> objects.</returns>
    [HttpGet("upcoming"), Authorize(Roles = "Holiday.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<HolidayDto>>>> GetUpcomingHolidays([FromQuery] int count = 10)
    {
        var result = await _commandQuery.Send(new GetUpcomingHolidaysQuery(count), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
