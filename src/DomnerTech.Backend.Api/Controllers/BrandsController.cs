using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Features.Brands;
using DomnerTech.Backend.Application.IRepo;
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
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status409Conflict)]
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
    [ProducesResponseType(typeof(BaseResponse<List<BrandDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<BrandDto>>>> GetAllBrands(
        [FromQuery(Name = "active_only")] bool activeOnly = true)
    {
        var result = await commandQuery.Send(new GetAllBrandsQuery(activeOnly), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
