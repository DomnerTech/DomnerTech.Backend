using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Helpers;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger,
    IUserRepo userRepo) : IRequestHandler<CreateUserCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new UserEntity
            {
                Id = ObjectId.GenerateNewId(),
                EmpId = ObjectId.GenerateNewId(),
                CompanyId = ObjectId.GenerateNewId(), // TODO: get from request
                Username = request.Username,
                PasswordHash = PasswordHashHelper.Hash(request.Pwd),
                IsDeleted = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await userRepo.CreateAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create user: {Error}", e.Message);
            throw;
        }

        return new BaseResponse<bool>
        {
            Data = true
        };
    }
}