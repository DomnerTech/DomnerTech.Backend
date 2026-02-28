using DomnerTech.Backend.Application.DTOs.Users;

namespace DomnerTech.Backend.Application.Services;

public interface IJwtService : IBaseService
{
    Task<string> CreateTokenAsync(UserDto user, CancellationToken cancellationToken = default);
}