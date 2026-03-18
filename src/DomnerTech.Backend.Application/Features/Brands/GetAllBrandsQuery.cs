using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Brands;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;

namespace DomnerTech.Backend.Application.Features.Brands;

/// <summary>
/// Query to get all brands.
/// </summary>
public sealed class GetAllBrandsQuery : OffsetPageRequest, IRequest<BaseResponse<OffsetPageResponse<BrandDto>>>;