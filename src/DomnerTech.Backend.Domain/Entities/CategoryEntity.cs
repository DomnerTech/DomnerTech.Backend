using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product category in the catalog.
/// </summary>
[MongoCollection("categories")]
public sealed class CategoryEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the category name (multi-language support).
    /// Key: language code (en, km, vi), Value: category name.
    /// </summary>
    public required Dictionary<string, string> Name { get; set; }

    /// <summary>
    /// Gets or sets the category description (multi-language support).
    /// </summary>
    public Dictionary<string, string>? Description { get; set; }

    /// <summary>
    /// Gets or sets the category slug for URL-friendly representation.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Gets or sets the parent category ID for hierarchical categories.
    /// </summary>
    public ObjectId? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this category is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets metadata for SEO and additional information.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
