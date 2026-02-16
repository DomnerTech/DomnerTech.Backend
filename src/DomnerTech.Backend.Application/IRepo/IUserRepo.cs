using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

public interface IUserRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(UserEntity entity, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
}