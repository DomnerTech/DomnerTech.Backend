using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.IRepo;

public interface IErrorMessageLocalizeRepo : IBaseRepo
{
    Task<string> ResolveAsync(string errorCode, string lang, CancellationToken cancellationToken = default);
    Task<ObjectId> CreateAsync(ErrorMessageLocalizeEntity entity, CancellationToken cancellationToken = default);
    Task<ErrorMessageLocalizeEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<ObjectId> UpdateAsync(ErrorMessageLocalizeEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
}