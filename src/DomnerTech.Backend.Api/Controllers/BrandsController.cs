using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Features.Brands;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing product brands.
/// </summary>
public sealed class BrandsController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new brand.
    /// </summary>
    /// <remarks>This endpoint requires the 'Brand.Write' role.</remarks>
    [HttpPost, Authorize(Roles = "Brand.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateBrand([FromBody] CreateBrandReqDto req)
    {
        var result = await commandQuery.Send(new CreateBrandCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all brands.
    /// </summary>
    /// <remarks>This endpoint requires the 'Brand.Read' role.</remarks>
    [HttpGet, Authorize(Roles = "Brand.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<BrandDto>>>> GetAllBrands(
        [FromQuery(Name = "cursor")] string? cursor,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "direction")] CursorDirection direction,
        [FromQuery(Name = "sort_by")] string sortBy,
        [FromQuery(Name = "include_total_count")] bool includeTotalCount,
        [FromQuery(Name = "active_only")] bool activeOnly = true)
    {
        var result = await commandQuery.Send(new GetAllBrandsQuery
        {
            ActiveOnly = activeOnly,
            PageSize = pageSize,
            SortKey = sortBy,
            Cursor = cursor,
            Direction = direction,
            IncludeTotalCount = includeTotalCount
        }, HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
