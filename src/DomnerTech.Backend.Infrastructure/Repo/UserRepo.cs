using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class UserRepo(
    IMongoDbContextFactory contextFactory, 
    ITenantService tenantService) : 
    BaseRepo<UserEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenantService),
    IUserRepo
{
    public async Task<ObjectId> CreateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<UserEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Where(i => i.Id == id && !i.IsDeleted);
        var user = await Collection.Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Where(i => i.Username == username && !i.IsDeleted);
        var user = await Collection.Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await Collection.FindAsync(TenantFilter(), cancellationToken: cancellationToken);
        return await users.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<UserEntity>.Update.Set(i => i.IsDeleted, true);
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}