using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Policies;

public class PolicyDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Desc { get; set; }
    public required HashSet<string> RequiredRoles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class PolicyExtensions
{
    public static PolicyDto ToDto(this PolicyEntity entity)
    {
        return new PolicyDto
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Desc = entity.Desc,
            RequiredRoles = entity.RequiredRoles,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static PolicyEntity ToEntity(this PolicyDto dto)
    {
        return new PolicyEntity
        {
            Id = dto.Id.ToObjectId(),
            Name = dto.Name,
            Desc = dto.Desc,
            RequiredRoles = dto.RequiredRoles,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}