using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Pagination.KeySetPaging;

public sealed class CompositeCursor
{
    public ObjectId TenantId { get; init; }
    public Dictionary<string, object> Values { get; init; } = [];
}