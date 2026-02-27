using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class RoleRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<RoleEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IRoleRepo
{
    public async Task<ObjectId> CreateAsync(RoleEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<bool> CheckRoleNamesAsync(HashSet<string> names, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.In(i => i.Name, names);
        var existingNames = await Collection
            .Find(TenantFilter() & filter)
            .Project(x => x.Name)
            .ToListAsync(cancellationToken);

        return names.All(existingNames.Contains);
    }

    public async Task<RoleEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Eq(i => i.Id, id);
        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Eq(i => i.Name, name);
        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoleEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Eq(i => i.Id, id);
        await Collection.DeleteOneAsync(TenantFilter() & filter, cancellationToken);
    }
}