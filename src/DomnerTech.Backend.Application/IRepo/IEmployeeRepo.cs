using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

public interface IEmployeeRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(EmployeeEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(EmployeeEntity entity, CancellationToken cancellationToken = default);
    Task<EmployeeEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}