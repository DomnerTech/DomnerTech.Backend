using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public class PolicyRepo : IPolicyRepo
{
    private readonly IMongoCollection<PolicyEntity> _collection;
    public PolicyRepo(IMongoDbContextFactory contextFactory)
    {
        var context = contextFactory.Create(DatabaseNameConstant.DatabaseName);
        _collection = context.Database.GetCollection<PolicyEntity>();
    }
    public async Task<ObjectId> CreateAsync(PolicyEntity entity, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<PolicyEntity?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var policy = await _collection.Find(i => i.Name == name).FirstOrDefaultAsync(cancellationToken);
        return policy;
    }

    public async Task<IEnumerable<PolicyEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _collection.Find(Builders<PolicyEntity>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(PolicyEntity entity, CancellationToken cancellationToken)
    {
        var filter = Builders<PolicyEntity>.Filter.Eq(i => i.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken)
    {
        await _collection.DeleteOneAsync(i => i.Name == name, cancellationToken);
    }
}