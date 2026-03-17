using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using MongoDB.Driver;

namespace DomnerTech.Backend.Application.Features.Brands.Handlers;

/// <summary>
/// Handler for getting all brands.
/// </summary>
public sealed class GetAllBrandsQueryHandler(
    IKeysetPaginator<BrandEntity> paginator,
    ITenantService tenantService) : IRequestHandler<GetAllBrandsQuery, BaseResponse<KeysetPageResult<BrandDto>>>
{
    public async Task<BaseResponse<KeysetPageResult<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = tenantService.CompanyId.ToObjectId();
        var pagingFilter = request.ActiveOnly 
            ? Builders<BrandEntity>.Filter.Eq(i => i.IsActive, true)
            : Builders<BrandEntity>.Filter.Empty;

        var paging = await paginator.PaginateAsync(
            DatabaseNameConstant.DatabaseName,
            tenantId,
            request,
            pagingFilter,
            cancellationToken);

        return new BaseResponse<KeysetPageResult<BrandDto>>
        {
            Data = paging.ToDto(i => i.ToDto())
        };
    }
}
