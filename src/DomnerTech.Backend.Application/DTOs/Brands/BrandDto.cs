using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Brands;

/// <summary>
/// DTO for brand response.
/// </summary>
public sealed record BrandDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class BrandExtensions
{
    public static BrandDto ToDto(this BrandEntity brand)
    {
        return new BrandDto
        {
            Id = brand.Id.ToString(),
            Name = brand.Name,
            Description = brand.Description,
            LogoUrl = brand.LogoUrl,
            IsActive = brand.IsActive,
            CreatedAt = brand.CreatedAt,
            UpdatedAt = brand.UpdatedAt
        };
    }
}