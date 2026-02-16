using System.Reflection;
using DomnerTech.Backend.Application.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Application;

public static class ValidationStartupChecks
{
    public static void EnsureAllValidatorsRegistered(
        IServiceCollection services,
        Assembly applicationAssembly)
    {
        services.BuildServiceProvider();

        var validatorTypes = applicationAssembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces())
            .Where(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IValidator<>))
            .Select(i => i.GenericTypeArguments[0])
            .ToHashSet();

        var requestsRequiringValidation = applicationAssembly
            .GetTypes()
            .Where(t =>
                typeof(IValidatableRequest).IsAssignableFrom(t) &&
                t.IsClass);

        var missingValidators = requestsRequiringValidation
            .Where(r => !validatorTypes.Contains(r))
            .Select(r => r.Name)
            .ToList();

        if (missingValidators.Count > 0)
        {
            throw new InvalidOperationException(
                $"Missing FluentValidation validators for: {string.Join(", ", missingValidators)}");
        }
    }
}