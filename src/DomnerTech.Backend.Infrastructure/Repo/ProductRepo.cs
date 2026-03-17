using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Pagination.OffsetPaging;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for Product entity.
/// </summary>
public sealed class ProductRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<ProductEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IProductRepo
{
    public async Task<ObjectId> CreateAsync(ProductEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(ProductEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.Eq(x => x.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<ProductEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Eq(x => x.Id, id),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Eq("Sku.Code", sku),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.ElemMatch(x => x.Barcodes, b => b.Value == barcode),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<ProductEntity>> GetByCategoryIdAsync(ObjectId categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Eq(x => x.CategoryId, categoryId),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<ProductEntity>> GetByBrandIdAsync(ObjectId brandId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Eq(x => x.BrandId, brandId),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<ProductEntity>> GetByStatusAsync(ProductStatus status, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Eq(x => x.Status, status),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<ProductEntity>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.And(
            Builders<ProductEntity>.Filter.Or(
                Builders<ProductEntity>.Filter.Regex("Name.en", new BsonRegularExpression(searchTerm, "i")),
                Builders<ProductEntity>.Filter.Regex("Name.km", new BsonRegularExpression(searchTerm, "i")),
                Builders<ProductEntity>.Filter.Regex("Name.vi", new BsonRegularExpression(searchTerm, "i")),
                Builders<ProductEntity>.Filter.Regex("Sku.Code", new BsonRegularExpression(searchTerm, "i"))
            ),
            Builders<ProductEntity>.Filter.Eq(x => x.IsDeleted, false)
        );
        return await Collection.Find(TenantFilter() & filter)
            .Limit(50)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, ObjectId deletedBy, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<ProductEntity>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.DeletedBy, deletedBy)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);
        
        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }
}
