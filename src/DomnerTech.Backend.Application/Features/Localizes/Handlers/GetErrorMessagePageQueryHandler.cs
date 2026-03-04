using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Pagination;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.Features.Localizes.Handlers;

public sealed class GetErrorMessagePageQueryHandler(
    IKeysetPaginator<ErrorMessageLocalizeEntity> paginator,
    ITenantService tenantService) : IRequestHandler<GetErrorMessagePageQuery, BaseResponse<KeysetPageResult<ErrorMessageLocalizeDto>>>
{
    public async Task<BaseResponse<KeysetPageResult<ErrorMessageLocalizeDto>>> Handle(GetErrorMessagePageQuery request, CancellationToken cancellationToken)
    {
        var tenantId = tenantService.CompanyId.ToObjectId();
        var paging = await paginator.PaginateAsync(
            DatabaseNameConstant.DatabaseName,
            tenantId,
            request,
            ct: cancellationToken);
        return new BaseResponse<KeysetPageResult<ErrorMessageLocalizeDto>>
        {
            Data = paging.ToDto(i => i.ToDto())
        };
    }
}