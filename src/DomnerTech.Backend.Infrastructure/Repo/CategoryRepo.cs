using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for Category entity.
/// </summary>
public sealed class CategoryRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<CategoryEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ICategoryRepo
{
    public async Task<ObjectId> CreateAsync(CategoryEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(CategoryEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<CategoryEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.And(
            Builders<CategoryEntity>.Filter.Eq(x => x.Id, id),
            Builders<CategoryEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoryEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.And(
            Builders<CategoryEntity>.Filter.Eq(x => x.Slug, slug),
            Builders<CategoryEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CategoryEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.And(
            Builders<CategoryEntity>.Filter.Eq(x => x.IsActive, true),
            Builders<CategoryEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CategoryEntity>> GetByParentIdAsync(ObjectId parentId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.And(
            Builders<CategoryEntity>.Filter.Eq(x => x.ParentCategoryId, parentId),
            Builders<CategoryEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CategoryEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<CategoryEntity>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedBy, deletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
