namespace DomnerTech.Backend.Application.DTOs.Products;

/// <summary>
/// DTO for creating a brand.
/// </summary>
public sealed record CreateBrandReqDto
{
    /// <summary>
    /// Brand name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Brand description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Brand slug for URL-friendly representation.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Brand logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Brand website URL.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO for brand response.
/// </summary>
public sealed record BrandDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Slug { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
