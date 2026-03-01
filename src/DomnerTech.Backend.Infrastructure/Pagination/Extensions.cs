using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Infrastructure.Pagination;

public static class Extensions
{
    public static IServiceCollection AddPagination(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton<ICursorSerializer>(new HmacCursorSerializer(appSettings.MongoDatabases.Paging.SecretKey));
        services.AddScoped(typeof(IKeysetPaginator<>), typeof(MongoKeysetPaginator<>));
        services.AddSingleton(typeof(SortProfile<>));
        return services;
    }
}