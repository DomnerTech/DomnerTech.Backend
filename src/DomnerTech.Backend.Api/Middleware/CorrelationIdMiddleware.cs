using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Exceptions;
using Elastic.Apm;
using Serilog.Context;

namespace DomnerTech.Backend.Api.Middleware;

public sealed class CorrelationIdMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue(HeaderConstants.CorrelationId, out var correlationIdValue))
        {
            throw new CorrelationIdRequiredException();
        }
        
        var correlationId = correlationIdValue.ToString();
        // Set APM label with correlation ID
        Agent.Tracer.CurrentTransaction?.SetLabel("correlationId", correlationId);
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimConstant.UserId)?.Value ?? string.Empty;
        Agent.Tracer.CurrentTransaction?.SetLabel("userId", userId);
        context.Items[HeaderConstants.CorrelationContextKey] = correlationId;

        context.Response.Headers[HeaderConstants.CorrelationId] = correlationId;
        using (LogContext.PushProperty("UserId", userId))
        {
            await next(context);
        }
    }
}