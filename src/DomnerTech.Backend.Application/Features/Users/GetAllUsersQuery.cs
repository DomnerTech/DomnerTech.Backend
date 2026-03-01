using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Pagination;

namespace DomnerTech.Backend.Application.Features.Users;

public sealed class GetAllUsersQuery : 
    KeysetPageRequest,
    IRequest<BaseResponse<KeysetPageResult<UserDto>>>,
    ILogReqCreator,
    IValidatableRequest;