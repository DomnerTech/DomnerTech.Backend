using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product brand in the catalog.
/// </summary>
[MongoCollection("brands")]
public sealed class BrandEntity : IBaseEntity, ITenantEntity
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
    /// Gets or sets the brand logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this brand is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
