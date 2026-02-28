using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;

namespace DomnerTech.Backend.Application.Features.Users;

public record GetUserByUsernameQuery(string Username) : IRequest<BaseResponse<UserDto?>>, ILogReqCreator, IValidatableRequest;