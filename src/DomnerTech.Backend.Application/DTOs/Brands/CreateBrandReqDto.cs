namespace DomnerTech.Backend.Application.DTOs.Brands;

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
    /// Brand logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }
}