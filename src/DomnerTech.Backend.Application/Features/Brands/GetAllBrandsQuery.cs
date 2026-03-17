using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;

namespace DomnerTech.Backend.Application.Features.Brands;

/// <summary>
/// Query to get all brands.
/// </summary>
public sealed class GetAllBrandsQuery :
    KeysetPageRequest,
    IRequest<BaseResponse<KeysetPageResult<BrandDto>>>
{
    public bool ActiveOnly { get; set; }
}