using Microsoft.AspNetCore.Authorization;

namespace DomnerTech.Backend.Api.Auth;

public sealed class AdminRoleBypassHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.Identity?.IsAuthenticated != true)
            return Task.CompletedTask;

        var isAdmin = context.User.IsInRole("Admin");

        if (!isAdmin)
            return Task.CompletedTask;

        // Mark ALL pending requirements as succeeded
        foreach (var requirement in context.PendingRequirements.ToList())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}