namespace DomnerTech.Backend.Application.DTOs.Products;

/// <summary>
/// DTO for creating a category.
/// </summary>
public sealed record CreateCategoryReqDto
{
    /// <summary>
    /// Category name in multiple languages.
    /// </summary>
    public required Dictionary<string, string> Name { get; set; }

    /// <summary>
    /// Category description in multiple languages.
    /// </summary>
    public Dictionary<string, string>? Description { get; set; }

    /// <summary>
    /// Category slug for URL-friendly representation.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Parent category ID for hierarchical categories.
    /// </summary>
    public string? ParentCategoryId { get; set; }

    /// <summary>
    /// Category image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO for category response.
/// </summary>
public sealed record CategoryDto : IBaseDto
{
    public required string Id { get; set; }
    public required Dictionary<string, string> Name { get; set; }
    public Dictionary<string, string>? Description { get; set; }
    public required string Slug { get; set; }
    public string? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
