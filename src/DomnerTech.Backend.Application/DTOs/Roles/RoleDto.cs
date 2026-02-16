using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Roles;

public class RoleDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Desc { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class RoleExtensions
{
    public static RoleEntity ToEntity(this RoleDto dto)
    {
        return new RoleEntity
        {
            Id = dto.Id.ToObjectId(),
            Name = dto.Name,
            Desc = dto.Desc,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
    
    public static RoleDto ToDto(this RoleEntity entity)
    {
        return new RoleDto
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Desc = entity.Desc,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}