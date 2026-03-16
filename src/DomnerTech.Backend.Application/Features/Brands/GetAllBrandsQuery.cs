using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Brands;

/// <summary>
/// Query to get all brands.
/// </summary>
public sealed record GetAllBrandsQuery(bool ActiveOnly = true) :
    IRequest<BaseResponse<List<BrandDto>>>;
