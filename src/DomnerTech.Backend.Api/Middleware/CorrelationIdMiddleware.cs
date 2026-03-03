using DomnerTech.Backend.Application.Constants;
using Elastic.Apm;
using Serilog.Context;

namespace DomnerTech.Backend.Api.Middleware;

public sealed class CorrelationIdMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = context.Request.Headers[HeaderConstants.CorrelationId].ToString();
        // Set APM label with correlation ID
        Agent.Tracer.CurrentTransaction?.SetLabel("correlationId", correlationId);
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId)?.Value ?? string.Empty;
        Agent.Tracer.CurrentTransaction?.SetLabel("userId", userId);
        using (LogContext.PushProperty("UserId", userId))
        {
            await next(context);
        }
    }
}