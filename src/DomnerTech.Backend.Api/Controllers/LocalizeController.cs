using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;
using DomnerTech.Backend.Application.Features.Localizes;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class LocalizeController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    [HttpPost("error-messages/upsert"), Authorize(Roles = "Localize.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> ErrorMessageLocalizeUpsert(
        [FromBody] ErrorMessageLocalizeUpsertReqDto r)
    {
        var res = await commandQuery.Send(
            new ErrorMessageLocalizeUpsertCommand(r.Key, r.Messages),
            HttpContext.RequestAborted);
        return await ReturnJson(res);
    }

    /// <summary>
    /// Retrieves a paginated list of localized error messages, supporting cursor-based navigation and sorting options.
    /// </summary>
    /// <remarks>This endpoint requires the 'Localize.Read' role. Cursor-based pagination enables efficient
    /// navigation through large result sets. Sorting and direction parameters allow flexible result ordering.</remarks>
    /// <param name="cursor">The cursor representing the position in the result set from which to continue retrieving error messages. Can be
    /// null to start from the beginning.</param>
    /// <param name="pageSize">The maximum number of error messages to return in the response. Must be a positive integer.</param>
    /// <param name="direction">The direction in which to paginate the results. Specify forward or backward to navigate through the result set.</param>
    /// <param name="sortBy">The field by which to sort the error messages. Must correspond to a valid sortable property.</param>
    /// <param name="includeTotalCount">Specifies whether to include the total count of error messages in the response. Set to <see langword="true"/> to
    /// include the count; otherwise, <see langword="false"/>.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="BaseResponse{T}"/> with a <see
    /// cref="KeysetPageResult{T}"/> of <see cref="ErrorMessageLocalizeDto"/> objects. The result includes the requested
    /// page of error messages and, if specified, the total count.</returns>
    [HttpGet("error-messages"), Authorize(Roles = "Localize.Read")]
    public async Task<ActionResult<BaseResponse<KeysetPageResult<ErrorMessageLocalizeDto>>>> GetErrorMessageLocalize(
        [FromQuery(Name = "cursor")] string? cursor,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "direction")] CursorDirection direction,
        [FromQuery(Name = "sort_by")] string sortBy,
        [FromQuery(Name = "include_total_count")] bool includeTotalCount)
    {
        var res = await commandQuery.Send(new GetErrorMessagePageQuery
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