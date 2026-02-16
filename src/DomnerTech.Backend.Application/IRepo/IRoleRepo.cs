using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

public interface IRoleRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(RoleEntity entity, CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateAsync(RoleEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
}