using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DomnerTech.Backend.Api.Auth;

public sealed class PolicyProvider(
    IOptions<AuthorizationOptions> options,
    IPolicyRepo policyRepo)
    : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback = new(options);

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await policyRepo.GetByNameAsync(policyName, CancellationToken.None);

        if (policy == null)
            return await _fallback.GetPolicyAsync(policyName);

        var builder = new AuthorizationPolicyBuilder();
        builder.AddRequirements(
            new DynamicRoleRequirement(policy.RequiredRoles));

        return builder.Build();
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallback.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallback.GetFallbackPolicyAsync();
    }
}