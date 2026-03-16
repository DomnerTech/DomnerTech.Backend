namespace DomnerTech.Backend.Application.DTOs.Inventory;

/// <summary>
/// DTO for creating a warehouse.
/// </summary>
public sealed record CreateWarehouseReqDto
{
    /// <summary>
    /// Warehouse name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Warehouse code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Warehouse type.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Warehouse address.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// City.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// Country.
    /// </summary>
    public required string Country { get; set; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Contact email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Warehouse manager ID.
    /// </summary>
    public string? ManagerId { get; set; }

    /// <summary>
    /// Is this the default warehouse.
    /// </summary>
    public bool IsDefault { get; set; }
}

/// <summary>
/// DTO for warehouse response.
/// </summary>
public sealed record WarehouseDto : IBaseDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ManagerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
