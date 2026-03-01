using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Features.Users;
using DomnerTech.Backend.Application.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

public sealed class UserController(ICommandQuery commandQuery) : BaseApiController
{
    [HttpGet("get-me")]
    public async Task<ActionResult<BaseResponse<UserDto>>> GetMe()
    {
        var user = await commandQuery.Send(new GetUserQuery(UserReqId), HttpContext.RequestAborted);
        return user.ReturnJson();
    }

    [HttpPost, Authorize(Roles = "User.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> CreateUser([FromBody] CreateUserDto r)
    {
        var result = await commandQuery.Send(new CreateUserCommand(r.Username, r.Pwd), HttpContext.RequestAborted);
        return result.ReturnJson();
    }

    /// <summary>
    /// Retrieves a paginated list of users based on the specified query parameters.
    /// </summary>
    /// <remarks>Requires the 'User.Read' role for authorization. The method supports cursor-based pagination
    /// and sorting to efficiently retrieve large user datasets.</remarks>
    /// <param name="cursor">An optional cursor value that indicates the starting point for pagination. If null, retrieval begins from the
    /// first item.</param>
    /// <param name="pageSize">The maximum number of users to return in the response. Must be a positive integer.</param>
    /// <param name="direction">The direction in which to paginate the results. Specify forward or backward to navigate through the user list.</param>
    /// <param name="sortBy">The field by which to sort the user results. Common values include user attributes such as 'id', 'username' and 'createdAt'.</param>
    /// <param name="includeTotalCount">Specifies whether to include the total count of users in the response. Set to <see langword="true"/> to include
    /// the count; otherwise, <see langword="false"/>.</param>
    /// <returns>
    /// An asynchronous operation that returns an <see cref="ActionResult"/> containing a <see cref="BaseResponse{T}"/> of <see langword="bool"/> 
    /// with the paginated user data and, if requested, the total user count.
    /// </returns>
    [HttpGet("all"), Authorize(Roles = "User.Read")]
    public async Task<ActionResult<BaseResponse<bool>>> GetAllUser(
        [FromQuery(Name = "cursor")] string? cursor,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "direction")] CursorDirection direction,
        [FromQuery(Name = "sort_by")] string sortBy,
        [FromQuery(Name = "include_total_count")] bool includeTotalCount)
    {
        var result = await commandQuery.Send(new GetAllUsersQuery
        {
            Cursor = cursor,
            Direction = direction,
            IncludeTotalCount = includeTotalCount,
            PageSize = pageSize,
            SortKey = sortBy
        }, HttpContext.RequestAborted);
        return result.ReturnJson();
    }
}