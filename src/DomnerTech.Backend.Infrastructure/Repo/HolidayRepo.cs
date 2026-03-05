using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

/// <summary>
/// Repository implementation for holiday operations.
/// </summary>
public sealed class HolidayRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<HolidayEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), IHolidayRepo
{
    public async Task<ObjectId> CreateAsync(HolidayEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task<int> BulkCreateAsync(List<HolidayEntity> entities, CancellationToken cancellationToken = default)
    {
        await Collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        return entities.Count;
    }

    public async Task UpdateAsync(HolidayEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Eq(i => i.Id, id);
        var update = Builders<HolidayEntity>.Update
            .Set(i => i.IsDeleted, true)
            .Set(i => i.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(TenantFilter() & filter, update, cancellationToken: cancellationToken);
    }

    public async Task<HolidayEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Eq(i => i.Id, id) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<HolidayEntity>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        var filter = Builders<HolidayEntity>.Filter.Gte(i => i.Date, startDate) &
                     Builders<HolidayEntity>.Filter.Lte(i => i.Date, endDate) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<HolidayEntity>> GetUpcomingAsync(DateTime fromDate, int count, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Gte(i => i.Date, fromDate) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.Date)
            .Limit(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsOnDateAsync(DateTime date, ObjectId? excludeId = null, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Eq(i => i.Date, date.Date) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsDeleted, false);

        if (excludeId.HasValue)
        {
            filter &= Builders<HolidayEntity>.Filter.Ne(i => i.Id, excludeId.Value);
        }

        var count = await Collection.CountDocumentsAsync(TenantFilter() & filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<List<HolidayEntity>> GetInRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<HolidayEntity>.Filter.Gte(i => i.Date, startDate.Date) &
                     Builders<HolidayEntity>.Filter.Lte(i => i.Date, endDate.Date) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsActive, true) &
                     Builders<HolidayEntity>.Filter.Eq(i => i.IsDeleted, false);

        return await Collection.Find(TenantFilter() & filter)
            .SortBy(x => x.Date)
            .ToListAsync(cancellationToken);
    }
}
