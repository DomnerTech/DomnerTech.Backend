using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Pagination;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using MongoDB.Driver;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class GetAllUsersQueryHandler(
    IKeysetPaginator<UserEntity> paginator,
    ITenantService tenantService) : IRequestHandler<GetAllUsersQuery, BaseResponse<KeysetPageResult<UserDto>>>
{
    public async Task<BaseResponse<KeysetPageResult<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var tenantId = tenantService.CompanyId.ToObjectId();
        var pagingFilter = Builders<UserEntity>.Filter.Where(i => !i.IsDeleted);
        var paging = await paginator.PaginateAsync(
            DatabaseNameConstant.DatabaseName,
            tenantId,
            request,
            pagingFilter,
            cancellationToken);
        return new BaseResponse<KeysetPageResult<UserDto>>
        {
            Data = paging.ToDto(i => i.ToDto())
        };
    }
}