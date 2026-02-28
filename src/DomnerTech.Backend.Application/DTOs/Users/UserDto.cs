using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Users;

public class UserDto : IBaseDto, ITenantDto
{
    public required string Username { get; set; }
    public required string EmpId { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public required string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string CompanyId { get; set; }
    public string PwdHash { get; set; } = string.Empty;
    public HashSet<string> Roles { get; set; } = [];
}

public static class UserExtensions
{
    public static UserEntity ToEntity(this UserDto dto)
    {
        return new UserEntity
        {
            Id = dto.Id.ToObjectId(),
            EmpId = dto.EmpId.ToObjectId(),
            CompanyId = dto.CompanyId.ToObjectId(),
            Username = dto.Username,
            LastLoginAt = dto.LastLoginAt,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            PasswordHash = dto.PwdHash,
            Roles = dto.Roles
        };
    }

    public static UserDto ToDto(this UserEntity entity)
    {
        return new UserDto
        {
            Id = entity.Id.ToString(),
            EmpId = entity.EmpId.ToString(),
            CompanyId = entity.CompanyId.ToString(),
            Username = entity.Username,
            LastLoginAt = entity.LastLoginAt,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Roles = entity.Roles,
            PwdHash = entity.PasswordHash
        };
    }
}