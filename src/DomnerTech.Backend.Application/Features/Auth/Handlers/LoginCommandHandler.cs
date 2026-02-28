using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Auth;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Features.Users;
using DomnerTech.Backend.Application.Helpers;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;

namespace DomnerTech.Backend.Application.Features.Auth.Handlers;

public sealed class LoginCommandHandler(
    IJwtService jwtService,
    ICommandQuery commandQuery,
    IUserRepo userRepo) : IRequestHandler<LoginCommand, BaseResponse<LoginResDto>>
{
    public async Task<BaseResponse<LoginResDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await commandQuery.Send(new GetUserByUsernameQuery(request.Username), cancellationToken);
        if (!user.IsSuccess || !PasswordHashHelper.Verify(request.Pwd, user.Data!.PwdHash))
        {
            throw new InvalidCredentialException();
        }

        var token = await jwtService.CreateTokenAsync(user.Data, cancellationToken);
        user.Data.LastLoginAt = DateTime.UtcNow;
        var userEntity = user.Data.ToEntity();
        await userRepo.UpdateAsync(userEntity, cancellationToken);
        // how to set the token in cookie

        return new BaseResponse<LoginResDto>
        {
            Data = new LoginResDto
            {
                Token = token,
                User = user.Data
            }
        };
    }
}