namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a product.
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// Product is in draft state and not visible to customers.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Product is active and available for sale.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Product is discontinued and no longer available for sale.
    /// </summary>
    Discontinued = 2,

    /// <summary>
    /// Product is out of stock across all warehouses.
    /// </summary>
    OutOfStock = 3
}
