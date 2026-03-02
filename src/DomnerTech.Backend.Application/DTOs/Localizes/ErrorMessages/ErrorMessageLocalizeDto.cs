using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Localizes.ErrorMessages;

public class ErrorMessageLocalizeDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Key { get; set; }
    public required Dictionary<string, string> Messages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class ErrorMessageLocalizeExtensions
{
    public static ErrorMessageLocalizeEntity ToEntity(this ErrorMessageLocalizeDto dto) => new()
    {
        Id = dto.Id.ToObjectId(),
        Key = dto.Key,
        Messages = dto.Messages,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt
    };

    public static ErrorMessageLocalizeDto ToDto(this ErrorMessageLocalizeEntity entity) => new()
    {
        Id = entity.Id.ToString(),
        Key = entity.Key,
        Messages = entity.Messages,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}