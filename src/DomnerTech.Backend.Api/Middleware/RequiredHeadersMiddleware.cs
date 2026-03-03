using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Enums;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.Api.Middleware;

public class RequiredHeadersMiddleware(
    RequestDelegate next,
    IErrorMessageLocalizeRepo errorMessageLocalizeRepo,
    ILogger<RequiredHeadersMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip validation for health check endpoint
        if (context.Request.Path == "/healthz")
        {
            await next(context);
            return;
        }

        var platformHeader = context.Request.Headers[HeaderConstants.Platform].ToString();
        var languageHeader = context.Request.Headers[HeaderConstants.Lang].ToString();
        var correlationIdHeader = context.Request.Headers[HeaderConstants.CorrelationId].ToString();

        // Validate platform header
        if (string.IsNullOrWhiteSpace(correlationIdHeader))
        {
            logger.LogWarning("Missing required header: {HeaderName}", HeaderConstants.CorrelationId);
            await WriteErrorResponseJsonAsync(context, languageHeader);
            return;
        }

        // Validate platform header
        if (string.IsNullOrWhiteSpace(platformHeader))
        {
            logger.LogWarning("Missing required header: {HeaderName}", HeaderConstants.Platform);
            await WriteErrorResponseJsonAsync(context, languageHeader);
            return;
        }

        // Validate language header
        if (string.IsNullOrWhiteSpace(languageHeader))
        {
            logger.LogWarning("Missing required header: {HeaderName}", HeaderConstants.Lang);
            await WriteErrorResponseJsonAsync(context, languageHeader);
            return;
        }

        // Validate language value
        if (!IsValidLanguage(languageHeader))
        {
            logger.LogWarning("Invalid language header value: {LanguageValue}", languageHeader);
            await WriteErrorResponseJsonAsync(context, languageHeader);
            return;
        }

        // Store headers in HttpContext.Items for use in controllers
        context.Items[HeaderConstants.Platform] = platformHeader;
        context.Items[HeaderConstants.Lang] = languageHeader;
        context.Items[HeaderConstants.CorrelationContextKey] = correlationIdHeader;
        context.Response.Headers[HeaderConstants.CorrelationId] = correlationIdHeader;

        await next(context);
    }

    private static bool IsValidLanguage(string language)
    {
        return language.Equals(nameof(LanguageSupportType.Km), StringComparison.OrdinalIgnoreCase) ||
               language.Equals(nameof(LanguageSupportType.En), StringComparison.OrdinalIgnoreCase);
    }

    private async Task WriteErrorResponseJsonAsync(HttpContext context, string languageHeader)
    {
        var res = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorCode = ErrorCodes.HeaderMissing
            }
        };
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";
        res.Status.Desc = await errorMessageLocalizeRepo.ResolveAsync(
            ErrorCodes.HeaderMissing,
            string.IsNullOrEmpty(languageHeader) ? nameof(LanguageSupportType.En).ToLower() : languageHeader);
        await context.Response.WriteAsJsonAsync(res);
    }
}