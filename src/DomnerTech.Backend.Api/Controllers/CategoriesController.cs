using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Features.Categories;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing product categories.
/// </summary>
public sealed class CategoriesController(
    ICommandQuery commandQuery,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo) : BaseApiController(errorMessageLocalizeRepo)
{
    /// <summary>
    /// Creates a new product category.
    /// </summary>
    /// <remarks>
    /// This endpoint requires the 'Category.Write' role. 
    /// Supports hierarchical categories with parent-child relationships.
    /// Supports multi-language names and descriptions.
    /// </remarks>
    [HttpPost, Authorize(Roles = "Category.Write")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BaseResponse<string>>> CreateCategory([FromBody] CreateCategoryReqDto req)
    {
        var result = await commandQuery.Send(new CreateCategoryCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <remarks>This endpoint requires the 'Category.Read' role.</remarks>
    [HttpGet, Authorize(Roles = "Category.Read")]
    [ProducesResponseType(typeof(BaseResponse<List<CategoryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<CategoryDto>>>> GetAllCategories(
        [FromQuery(Name = "active_only")] bool activeOnly = true)
    {
        var result = await commandQuery.Send(new GetAllCategoriesQuery(activeOnly), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
