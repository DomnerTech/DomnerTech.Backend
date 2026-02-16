using System.Collections.Concurrent;
using Bas24.CommandQuery;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Application.Pipelines;

public sealed class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly ConcurrentDictionary<Type, IValidator<TRequest>[]> Cache = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validators = Cache.GetOrAdd(typeof(TRequest), _ => [.. serviceProvider.GetServices<IValidator<TRequest>>()]);

        if (validators.Length == 0)
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}
