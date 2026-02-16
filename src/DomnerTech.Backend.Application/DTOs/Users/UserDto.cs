using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Users;

public class UserDto : IBaseDto, IAuditDto, ISoftDeleteDto, ITenantDto
{
    public required string Username { get; set; }
    public required string EmpId { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public required string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public required string CompanyId { get; set; }
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
            PasswordHash = dto.PasswordHash,
            LastLoginAt = dto.LastLoginAt,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            UpdatedBy = string.IsNullOrEmpty(dto.UpdatedBy) ? null : dto.UpdatedBy.ToObjectId(),
            DeletedBy = string.IsNullOrEmpty(dto.DeletedBy) ? null : dto.DeletedBy.ToObjectId(),
            IsDeleted = dto.IsDeleted
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
            PasswordHash = entity.PasswordHash,
            LastLoginAt = entity.LastLoginAt,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy?.ToString(),
            DeletedBy = entity.DeletedBy?.ToString(),
            IsDeleted = entity.IsDeleted
        };
    }
}