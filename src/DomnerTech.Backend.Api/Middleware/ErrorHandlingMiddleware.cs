using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Helpers;
using DomnerTech.Backend.Application.Json;
using FluentValidation;

namespace DomnerTech.Backend.Api.Middleware;

public sealed class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteProblemDetails(context, ex);
        }
        catch (BaseErrorException ex)
        {
            await WriteProblemDetails(context, ex);
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                throw;
            }

            logger.LogError(ex, "Unhandled exception");
            await WriteProblemDetails(context, ex);
        }
    }

    private static async Task WriteProblemDetails(
        HttpContext context,
        ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName.ToSnakeCase())
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Desc = "Validation failed",
                ErrorCode = ErrorCodes.Validation,
                Errors = errors
            }
        };

        context.Response.StatusCode = problem.Status.StatusCode;
        await context.Response.WriteAsJsonAsync(problem, DefaultJsonSerializerSettings.SnakeCase);
    }

    private static async Task WriteProblemDetails(
        HttpContext context,
        BaseErrorException ex)
    {
        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = ex.StatusCode,
                Desc = ex.Message,
                ErrorCode = ex.Code
            }
        };

        context.Response.StatusCode = ex.StatusCode;
        await context.Response.WriteAsJsonAsync(problem, DefaultJsonSerializerSettings.SnakeCase);
    }

    private static async Task WriteProblemDetails(
        HttpContext context,
        Exception _)
    {
        var problem = new BaseResponse
        {
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Desc = "Unexpected error occurred",
                ErrorCode = ErrorCodes.Internal
            }
        };

        context.Response.StatusCode = problem.Status.StatusCode;
        await context.Response.WriteAsJsonAsync(problem, DefaultJsonSerializerSettings.SnakeCase);
    }
}