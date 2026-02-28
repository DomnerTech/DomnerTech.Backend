using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class GetAllUsersQueryHandler(IUserRepo userRepo) : IRequestHandler<GetAllUsersQuery, BaseResponse<IEnumerable<UserDto>>>
{
    public async Task<BaseResponse<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepo.GetAllAsync(cancellationToken);
        return new BaseResponse<IEnumerable<UserDto>>
        {
            Data = users.Select(u => u.ToDto())
        };
    }
}