using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for ProductVariant entity.
/// </summary>
public sealed class ProductVariantRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<ProductVariantEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IProductVariantRepo
{
    public async Task<ObjectId> CreateAsync(ProductVariantEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(ProductVariantEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductVariantEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<ProductVariantEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductVariantEntity>.Filter.And(
            Builders<ProductVariantEntity>.Filter.Eq(x => x.Id, id),
            Builders<ProductVariantEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<ProductVariantEntity>> GetByProductIdAsync(ObjectId productId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductVariantEntity>.Filter.And(
            Builders<ProductVariantEntity>.Filter.Eq(x => x.ProductId, productId),
            Builders<ProductVariantEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductVariantEntity?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductVariantEntity>.Filter.And(
            Builders<ProductVariantEntity>.Filter.Eq("Sku.Code", sku),
            Builders<ProductVariantEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductVariantEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<ProductVariantEntity>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedBy, deletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
