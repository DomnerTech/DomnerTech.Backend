using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Application.Pagination;

public interface IKeysetPaginator<T>
{
    Task<KeysetPageResult<T>> PaginateAsync(
        string dbName,
        ObjectId tenantId,
        KeysetPageRequest request,
        FilterDefinition<T>? userFilter = null,
        CancellationToken ct = default);
}