using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Services;

namespace DomnerTech.Backend.Api.Middleware;

public class TenantMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var companyId = context.User.FindFirst(ClaimConstant.CompanyId)?.Value;

            if (string.IsNullOrEmpty(companyId))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid tenant.");
                return;
            }

            tenantService.SetTenant(companyId);
        }

        await next(context);
    }
}