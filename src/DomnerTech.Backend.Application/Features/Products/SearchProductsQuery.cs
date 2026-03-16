using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Query to search products by name or SKU.
/// </summary>
public sealed record SearchProductsQuery(string SearchTerm) :
    IRequest<BaseResponse<List<ProductDto>>>;
