using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class UserRepo : IUserRepo
{
    private readonly IMongoCollection<UserEntity> _collection;
    public UserRepo(IMongoDbContextFactory contextFactory)
    {
        var context = contextFactory.Create(DatabaseNameConstant.DatabaseName);
        _collection = context.Database.GetCollection<UserEntity>();
    }

    public async Task<ObjectId> CreateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<UserEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var user = await _collection.Find(i => i.Id == id && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _collection.Find(i => i.Username == username && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Where(i => i.Id == entity.Id);
        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Where(i => i.Id == id);
        var update = Builders<UserEntity>.Update.Set(i => i.IsDeleted, true);
        await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}