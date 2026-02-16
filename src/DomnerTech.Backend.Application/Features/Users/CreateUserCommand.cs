using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Users;

public sealed record CreateUserCommand(string Username, string Pwd) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;