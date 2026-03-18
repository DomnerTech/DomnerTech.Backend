using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Brands;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;

namespace DomnerTech.Backend.Application.Features.Brands.Handlers;

/// <summary>
/// Handler for getting all brands.
/// </summary>
public sealed class GetAllBrandsQueryHandler(IBrandRepo brandRepo) : IRequestHandler<GetAllBrandsQuery, BaseResponse<OffsetPageResponse<BrandDto>>>
{
    public async Task<BaseResponse<OffsetPageResponse<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var paging = await brandRepo.GetPagedAsync(
            request: request,
            projection: e => e.ToDto(),
            cancellationToken: cancellationToken);

        return new BaseResponse<OffsetPageResponse<BrandDto>>
        {
            Data = paging
        };
    }
}
