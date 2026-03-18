using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Brands;
using DomnerTech.Backend.Application.Features.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing product brands.
/// </summary>
public sealed class BrandsController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Creates a new brand.
    /// </summary>
    /// <remarks>This endpoint requires the 'Brand.Write' role.</remarks>
    [HttpPost, Authorize(Roles = "Brand.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateBrand([FromBody] CreateBrandReqDto req)
    {
        var result = await _commandQuery.Send(new CreateBrandCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all brands.
    /// </summary>
    /// <remarks>This endpoint requires the 'Brand.Read' role.</remarks>
    [HttpPost("page"), Authorize(Roles = "Brand.Read")]
    public async Task<ActionResult<BaseResponse<IEnumerable<BrandDto>>>> GetAllBrands([FromBody] GetAllBrandsReqDto req)
    {
        var result = await _commandQuery.Send(new GetAllBrandsQuery
        {
            PageSize = req.PageSize,
            IncludeTotalCount = req.IncludeTotalCount,
            Filters = req.Filters,
            PageNumber = req.PageNumber,
            Sort = req.Sort
        }, HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
