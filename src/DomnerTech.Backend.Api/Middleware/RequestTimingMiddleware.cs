using System.Diagnostics;

namespace DomnerTech.Backend.Api.Middleware;

public sealed class RequestTimingMiddleware(ILogger<RequestTimingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var path = context.Request.Path.Value ?? "/";
            var factor = elapsedMs switch
            {
                < 100 => "fast",
                < 500 => "normal",
                < 1000 => "slow",
                _ => "critical"
            };

            logger.LogDebug("request-timing: {Path} {ElapsedMs}ms {Factor}", path, elapsedMs, factor);
        }
    }
}