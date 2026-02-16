using Microsoft.Extensions.DependencyInjection;
using Mobile.CleanArchProjectTemplate.Application.Services;

namespace DomnerTech.Backend.Infrastructure.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo<IBaseService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        return services;
    }
}