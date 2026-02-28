using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Helpers;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Users.Handlers;

public sealed class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger,
    IUserRepo userRepo,
    ITenantService tenantService) : IRequestHandler<CreateUserCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new UserEntity
            {
                Id = ObjectId.GenerateNewId(),
                EmpId = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                Username = request.Username,
                PasswordHash = PasswordHashHelper.Hash(request.Pwd),
                Roles = [],
                IsDeleted = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await userRepo.CreateAsync(entity, cancellationToken);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create user: {Error}", e.Message);
        }
        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}