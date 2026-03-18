using DomnerTech.Backend.Application;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;
using DomnerTech.Backend.Infrastructure.Pagination.KeysetPaging;
using DomnerTech.Backend.Infrastructure.Pagination.OffsetPaging;
using Microsoft.Extensions.DependencyInjection;

namespace DomnerTech.Backend.Infrastructure.Pagination;

public static class Extensions
{
    public static IServiceCollection AddPagination(this IServiceCollection services, AppSettings appSettings)
    {
        // Keyset pagination (existing)
        services.AddSingleton<ICursorSerializer>(new HmacCursorSerializer(appSettings.MongoDatabases.Paging.SecretKey));
        services.AddScoped(typeof(IKeysetPaginator<>), typeof(MongoKeysetPaginator<>));
        services.AddSingleton(typeof(SortProfile<>));

        // Offset pagination (new)
        services.AddScoped(typeof(IOffsetPaginator<,>), typeof(MongoOffsetPaginator<,>));
        services.AddSingleton<FieldValidationService>();

        return services;
    }
}