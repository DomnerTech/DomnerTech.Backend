using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.Services;

public interface IJwtService : IBaseService
{
    Task<string> CreateTokenAsync(UserEntity user, CancellationToken cancellationToken = default);
}