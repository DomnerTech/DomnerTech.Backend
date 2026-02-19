using DomnerTech.Backend.Application.Constants;
using Microsoft.AspNetCore.Authorization;

namespace DomnerTech.Backend.Api.Auth;

public sealed class DynamicRoleHandler
    : AuthorizationHandler<DynamicRoleRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DynamicRoleRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimConstant.UserId)?.Value;
        var companyId = context.User.FindFirst(ClaimConstant.CompanyId)?.Value;

        if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(companyId))
            return Task.CompletedTask;

        var policies = context.User.FindAll(ClaimConstant.Roles)
            .Select(p => p.Value);
        if (requirement.Roles.Any(i => policies.Contains(i)))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}