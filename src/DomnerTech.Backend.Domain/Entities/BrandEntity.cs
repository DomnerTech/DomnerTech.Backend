using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product brand in the catalog.
/// </summary>
[MongoCollection("brands")]
public sealed class BrandEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the brand name.
    /// </summary>
    [Sortable(alias: "name", order: 2)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the brand description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the brand slug for URL-friendly representation.
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Gets or sets the brand logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets the brand website URL.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this brand is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the display order for sorting.
    /// </summary>
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
