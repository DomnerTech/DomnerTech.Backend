using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Roles.Handlers;

public sealed class CreateRoleCommandHandler(
    ILogger<CreateRoleCommandHandler> logger, 
    IRoleRepo roleRepo) : IRequestHandler<CreateRoleCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await roleRepo.CreateAsync(new RoleEntity
            {
                Name = request.Name,
                Id = ObjectId.GenerateNewId(),
                Desc = request.Desc,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }, cancellationToken);
            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error create role: {Error}", e.Message);
            throw;
        }
    }
}