using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Categories;

/// <summary>
/// Query to get all categories.
/// </summary>
public sealed record GetAllCategoriesQuery(bool ActiveOnly = true) :
    IRequest<BaseResponse<List<CategoryDto>>>;
