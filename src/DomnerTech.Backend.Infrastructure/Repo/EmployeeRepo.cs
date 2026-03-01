using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class EmployeeRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<EmployeeEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IEmployeeRepo
{
    public async Task<ObjectId> CreateAsync(EmployeeEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(EmployeeEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<EmployeeEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<EmployeeEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<EmployeeEntity>.Filter.Eq(i => i.Id, id);
        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        var count = await Collection.CountDocumentsAsync(TenantFilter(), cancellationToken: cancellationToken);
        return (int)count;
    }
}