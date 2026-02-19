using Microsoft.AspNetCore.Authorization;

namespace DomnerTech.Backend.Api.Auth;

public class DynamicRoleRequirement(HashSet<string> roles) : IAuthorizationRequirement
{
    public HashSet<string> Roles { get; } = roles;
}