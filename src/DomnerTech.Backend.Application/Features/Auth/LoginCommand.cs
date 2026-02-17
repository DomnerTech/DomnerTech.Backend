using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Auth;

namespace DomnerTech.Backend.Application.Features.Auth;

public sealed record LoginCommand(string Username, string Pwd) :
    IRequest<BaseResponse<LoginResDto>>,
    ILogCreator, IValidatableRequest;