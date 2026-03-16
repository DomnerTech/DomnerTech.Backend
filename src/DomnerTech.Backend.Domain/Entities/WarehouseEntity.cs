using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a warehouse or store location.
/// </summary>
[MongoCollection("warehouses")]
public sealed class WarehouseEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse name.
    /// </summary>
    [Sortable(alias: "name", order: 2)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the warehouse code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Gets or sets the warehouse type (e.g., Main, Branch, Distribution Center).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets the warehouse address.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public required string Country { get; set; }

    /// <summary>
    /// Gets or sets the contact phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the contact email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the warehouse manager ID.
    /// </summary>
    public ObjectId? ManagerId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this warehouse is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this is the default/main warehouse.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets metadata for additional information.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
