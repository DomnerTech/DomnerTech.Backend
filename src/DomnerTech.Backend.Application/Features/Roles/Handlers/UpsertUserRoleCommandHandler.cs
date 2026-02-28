using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Users;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Features.Users;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Application.Features.Roles.Handlers;

public sealed class UpsertUserRoleCommandHandler(ICommandQuery commandQuery, IRoleRepo roleRepo, IUserRepo userRepo) : IRequestHandler<UpsertUserRoleCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpsertUserRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepo.GetByNameAsync(request.RoleName, cancellationToken);
        if (role is null) throw new NotFoundException($"Role '{request.RoleName}' not found!");
        var userDto = await commandQuery.Send(new GetUserQuery(request.UserId), cancellationToken);
        if (!userDto.IsSuccess)
            return new BaseResponse<bool>
            {
                Data = false,
                Status = userDto.Status
            };

        // ReSharper disable once CanSimplifySetAddingWithSingleCall
        if (userDto.Data.Roles.Contains(request.RoleName))
            throw new ConflictException($"User already have role '{request.RoleName}'");

        userDto.Data.Roles.Add(request.RoleName);
        await userRepo.UpdateAsync(userDto.Data.ToEntity(), cancellationToken);
        return new BaseResponse<bool>
        {
            Data = true
        };
    }
}