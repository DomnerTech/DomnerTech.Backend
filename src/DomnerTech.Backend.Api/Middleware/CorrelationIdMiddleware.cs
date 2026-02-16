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

        if (!context.Request.Headers.TryGetValue(HeaderConstants.UserId, out var appIdValue))
        {
            throw new UserIdRequiredException();
        }

        var correlationId = correlationIdValue.ToString();
        // Set APM label with correlation ID
        Agent.Tracer.CurrentTransaction?.SetLabel("correlationId", correlationId);
        Agent.Tracer.CurrentTransaction?.SetLabel("userId", appIdValue);
        context.Items[HeaderConstants.CorrelationContextKey] = correlationId;

        context.Response.Headers[HeaderConstants.CorrelationId] = correlationId;
        using (LogContext.PushProperty("UserId", appIdValue.ToString()))
        {
            await next(context);
        }
    }
}