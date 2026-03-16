using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a product bundle or kit containing multiple products.
/// </summary>
[MongoCollection("productBundles")]
public sealed class ProductBundleEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the bundle product ID.
    /// </summary>
    public required ObjectId BundleProductId { get; set; }

    /// <summary>
    /// Gets or sets the component product ID.
    /// </summary>
    public required ObjectId ComponentProductId { get; set; }

    /// <summary>
    /// Gets or sets the component product variant ID (if applicable).
    /// </summary>
    public ObjectId? ComponentVariantId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of this component in the bundle.
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this component is optional.
    /// </summary>
    public bool IsOptional { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}
