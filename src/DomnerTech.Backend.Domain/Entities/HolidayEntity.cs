using DomnerTech.Backend.Domain.Enums;
using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

/// <summary>
/// Represents a holiday in the company calendar.
/// </summary>
[MongoCollection("holidays")]
public sealed class HolidayEntity : IBaseEntity, ITenantEntity, IAuditEntity, ISoftDeleteEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the name of the holiday.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the holiday.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the holiday date.
    /// </summary>
    [Sortable(alias: "date", order: 2)]
    public required DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the type of holiday.
    /// </summary>
    public HolidayType Type { get; set; }

    /// <summary>
    /// Gets or sets whether this holiday is recurring annually.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Gets or sets the country code this holiday applies to (ISO 3166-1 alpha-2).
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the region/state this holiday applies to.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets whether this holiday is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}
