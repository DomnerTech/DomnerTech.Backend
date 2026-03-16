using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Query to get all products with optional filtering.
/// </summary>
public sealed record GetAllProductsQuery(
    string? CategoryId = null,
    string? BrandId = null,
    string? Status = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<BaseResponse<List<ProductDto>>>;
