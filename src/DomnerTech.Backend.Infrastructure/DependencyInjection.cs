using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Infrastructure.Caching;
using DomnerTech.Backend.Infrastructure.MongoDb;
using DomnerTech.Backend.Infrastructure.Repo;
using DomnerTech.Backend.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddCache(appSettings)
            .AddMongoDb()
            .AddRepo()
            .AddServices();

        services.AddHttpContextAccessor();
        services.AddScoped<ICorrelationContext, CorrelationContext>();
    }
}