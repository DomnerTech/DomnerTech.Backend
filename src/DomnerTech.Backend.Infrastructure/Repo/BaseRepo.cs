using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.Services;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public abstract class BaseRepo<T>(
    IMongoDatabase database,
    ITenantService? tenant = null) where T : class
{
    protected readonly IMongoCollection<T> Collection = database.GetCollection<T>();
    protected readonly ITenantService? Tenant = tenant;

    protected FilterDefinition<T> TenantFilter()
    {
        return Tenant != null 
            ? Builders<T>.Filter.Eq("companyId", Tenant.CompanyId.ToObjectId())
            : Builders<T>.Filter.Empty;
    }
}