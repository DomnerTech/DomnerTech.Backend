using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Policies.Handlers;

public sealed class CreatePolicyCommandHandler(
    ILogger<CreatePolicyCommandHandler> logger,
    IPolicyRepo policyRepo,
    IRoleRepo roleRepo) : IRequestHandler<CreatePolicyCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add Redis Caching for the policies and roles to improve performance
        
        var policy = await policyRepo.GetByNameAsync(request.Name, cancellationToken);
        if (policy != null)
        {
            throw new ConflictException($"Policy with name: '{request.Name}' already existed!");
        }

        if (!await roleRepo.CheckRoleNamesAsync(request.RoleNames, cancellationToken))
        {
            throw new NotFoundException("One or more of roles not existed!");
        }

        var id = await policyRepo.CreateAsync(new PolicyEntity
        {
            Id = ObjectId.GenerateNewId(),
            Name = request.Name,
            RequiredRoles = request.RoleNames,
            Desc = request.Desc,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);
        return new BaseResponse<string>
        {
            Data = id.ToString()
        };
    }
}