using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.Repo;

public sealed class LeaveBalanceRepo(
    IMongoDbContextFactory contextFactory,
    ITenantService tenant)
    : BaseRepo<LeaveBalanceEntity>(contextFactory.Create(DatabaseNameConstant.DatabaseName).Database, tenant), ILeaveBalanceRepo
{
    public async Task<ObjectId> CreateAsync(LeaveBalanceEntity entity, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(LeaveBalanceEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBalanceEntity>.Filter.Eq(i => i.Id, entity.Id);
        await Collection.ReplaceOneAsync(TenantFilter() & filter, entity, cancellationToken: cancellationToken);
    }

    public async Task<LeaveBalanceEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBalanceEntity>.Filter.Eq(i => i.Id, id);
        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<LeaveBalanceEntity?> GetByEmployeeAndTypeAsync(ObjectId employeeId, ObjectId leaveTypeId, int year, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBalanceEntity>.Filter.Eq(i => i.EmployeeId, employeeId) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.LeaveTypeId, leaveTypeId) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.Year, year) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.IsActive, true);

        return await Collection.Find(TenantFilter() & filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<LeaveBalanceEntity>> GetByEmployeeAsync(ObjectId employeeId, int year, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBalanceEntity>.Filter.Eq(i => i.EmployeeId, employeeId) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.Year, year) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.IsActive, true);

        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveBalanceEntity>> GetAllByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        var filter = Builders<LeaveBalanceEntity>.Filter.Eq(i => i.Year, year) &
                     Builders<LeaveBalanceEntity>.Filter.Eq(i => i.IsActive, true);

        return await Collection.Find(TenantFilter() & filter).ToListAsync(cancellationToken);
    }

    public async Task<bool> HasSufficientBalanceAsync(ObjectId employeeId, ObjectId leaveTypeId, int year, decimal requiredDays, CancellationToken cancellationToken = default)
    {
        var balance = await GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year, cancellationToken);
        if (balance == null) return false;

        var remaining = balance.Allowance.TotalAllowance + balance.Allowance.CarriedForwardDays - balance.Allowance.UsedDays;
        return remaining >= requiredDays;
    }
}
