using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class RoleRepo : IRoleRepo
{
    private readonly IMongoCollection<RoleEntity> _collection;
    public RoleRepo(IMongoDbContextFactory contextFactory)
    {
        var context = contextFactory.Create(DatabaseNameConstant.DatabaseName);
        _collection = context.Database.GetCollection<RoleEntity>();
    }

    public async Task<ObjectId> CreateAsync(RoleEntity entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<bool> CheckRoleNamesAsync(HashSet<string> names, CancellationToken cancellationToken = default)
    {
        // I want to check if all the role names in the "names" parameter exist in the db,
        // if any of them does not exist, return false, otherwise return true
        var filter = Builders<RoleEntity>.Filter.In(i => i.Name, names);
        var existingNames = await _collection
            .Find(filter)
            .Project(x => x.Name)
            .ToListAsync(cancellationToken);

        return names.All(existingNames.Contains);
    }

    public async Task<RoleEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(i => i.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(i => i.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoleEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Where(i => i.Id == entity.Id);
        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RoleEntity>.Filter.Where(i => i.Id == id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }
}