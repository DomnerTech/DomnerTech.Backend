using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

public interface IPolicyRepo : IBaseRepo
{
    Task<ObjectId> CreateAsync(PolicyEntity entity, CancellationToken cancellationToken);
    Task<PolicyEntity?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<PolicyEntity>> GetByNamesAsync(HashSet<string> names, CancellationToken cancellationToken);
    Task<IEnumerable<PolicyEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(PolicyEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(string name, CancellationToken cancellationToken);
}