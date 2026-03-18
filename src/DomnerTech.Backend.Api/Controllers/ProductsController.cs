using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Features.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomnerTech.Backend.Api.Controllers;

/// <summary>
/// Controller for managing products in the catalog.
/// </summary>
public sealed class ProductsController(ICommandQuery commandQuery) : BaseApiController(commandQuery)
{
    /// <summary>
    /// Creates a new product in the catalog.
    /// </summary>
    /// <remarks>
    /// This endpoint requires the 'Product.Write' role. 
    /// SKU will be auto-generated if not provided.
    /// Supports multi-language names and descriptions.
    /// Supports multi-currency pricing (USD, KHR, VND).
    /// </remarks>
    [HttpPost, Authorize(Roles = "Product.Write")]
    public async Task<ActionResult<BaseResponse<string>>> CreateProduct([FromBody] CreateProductReqDto req)
    {
        var result = await _commandQuery.Send(new CreateProductCommand(req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <remarks>This endpoint requires the 'Product.Write' role.</remarks>
    [HttpPut("{productId}"), Authorize(Roles = "Product.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateProduct(
        [FromRoute] string productId,
        [FromBody] CreateProductReqDto req)
    {
        var result = await _commandQuery.Send(new UpdateProductCommand(productId, req), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Deletes a product (soft delete).
    /// </summary>
    /// <remarks>This endpoint requires the 'Product.Write' role. Product cannot be deleted if it has active transactions or stock.</remarks>
    [HttpDelete("{productId}"), Authorize(Roles = "Product.Write")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteProduct([FromRoute] string productId)
    {
        var result = await _commandQuery.Send(new DeleteProductCommand(productId), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets a product by ID.
    /// </summary>
    /// <remarks>This endpoint requires the 'Product.Read' role.</remarks>
    [HttpGet("{productId}"), Authorize(Roles = "Product.Read")]
    public async Task<ActionResult<BaseResponse<ProductDto>>> GetProduct([FromRoute] string productId)
    {
        var result = await _commandQuery.Send(new GetProductByIdQuery(productId), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Gets all products with optional filtering.
    /// </summary>
    /// <remarks>This endpoint requires the 'Product.Read' role. Supports filtering by category, brand, and status.</remarks>
    [HttpGet, Authorize(Roles = "Product.Read")]
    public async Task<ActionResult<BaseResponse<List<ProductDto>>>> GetAllProducts(
        [FromQuery(Name = "category_id")] string? categoryId = null,
        [FromQuery(Name = "brand_id")] string? brandId = null,
        [FromQuery(Name = "status")] string? status = null,
        [FromQuery(Name = "page_number")] int pageNumber = 1,
        [FromQuery(Name = "page_size")] int pageSize = 20)
    {
        var result = await _commandQuery.Send(
            new GetAllProductsQuery(categoryId, brandId, status, pageNumber, pageSize),
            HttpContext.RequestAborted);
        return await ReturnJson(result);
    }

    /// <summary>
    /// Searches products by name or SKU.
    /// </summary>
    /// <remarks>This endpoint requires the 'Product.Read' role.</remarks>
    [HttpGet("search"), Authorize(Roles = "Product.Read")]
    public async Task<ActionResult<BaseResponse<List<ProductDto>>>> SearchProducts(
        [FromQuery(Name = "q")] string searchTerm)
    {
        var result = await _commandQuery.Send(new SearchProductsQuery(searchTerm), HttpContext.RequestAborted);
        return await ReturnJson(result);
    }
}
