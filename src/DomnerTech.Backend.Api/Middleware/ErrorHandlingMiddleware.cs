using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Helpers;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Json;

namespace DomnerTech.Backend.Api.Middleware;

public sealed class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger,
    IErrorMessageLocalizeRepo errorMessageResolver) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (FluentValidation.ValidationException ex)
        {
            await WriteValidation(context, ex);
        }
        catch (BaseErrorException ex)
        {
            await WriteBaseError(context, ex);
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                throw;
            }
            logger.LogError(ex, "Unhandled exception");
            await WriteInternal(context);
        }
    }

    private async Task WriteValidation(
        HttpContext context,
        FluentValidation.ValidationException ex)
    {
        var lang = context.GetCurrentLanguage();

        var errors = new Dictionary<string, string[]>();
        foreach (var group in ex.Errors.GroupBy(e => e.PropertyName.ToSnakeCase()))
        {
            var resolvedErrors = new List<string>();
            foreach (var error in group)
            {
                var resolvedMessage = await errorMessageResolver.ResolveAsync(error.ErrorCode, lang);
                resolvedErrors.Add(resolvedMessage);
            }

            var keys = group.Key.Split('.');
            errors[keys.Length > 1 ? keys[^1] : group.Key ] = [.. resolvedErrors];
        }

        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Desc = await errorMessageResolver.ResolveAsync(ErrorCodes.Validation, lang),
                ErrorCode = ErrorCodes.Validation,
                Errors = errors
            }
        };

        context.Response.StatusCode = problem.Status.StatusCode;

        await context.Response.WriteAsJsonAsync(
            problem,
            DefaultJsonSerializerSettings.SnakeCase);
    }

    private async Task WriteBaseError(
        HttpContext context,
        BaseErrorException ex)
    {
        var lang = context.GetCurrentLanguage();
        var desc = await errorMessageResolver.ResolveAsync(ex.Code, lang);
        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = ex.StatusCode,
                Desc = desc,
                ErrorCode = ex.Code
            }
        };

        context.Response.StatusCode = ex.StatusCode;

        await context.Response.WriteAsJsonAsync(
            problem,
            DefaultJsonSerializerSettings.SnakeCase);
    }

    private async Task WriteInternal(HttpContext context)
    {
        var lang = context.GetCurrentLanguage();
        var desc = await errorMessageResolver.ResolveAsync(ErrorCodes.SystemError, lang);
        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Desc = desc,
                ErrorCode = ErrorCodes.SystemError
            }
        };

        context.Response.StatusCode = problem.Status.StatusCode;

        await context.Response.WriteAsJsonAsync(
            problem,
            DefaultJsonSerializerSettings.SnakeCase);
    }
}