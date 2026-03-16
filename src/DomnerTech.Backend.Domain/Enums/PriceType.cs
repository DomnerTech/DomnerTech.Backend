namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the type of product pricing.
/// </summary>
public enum PriceType
{
    /// <summary>
    /// Regular retail price for individual customers.
    /// </summary>
    Retail = 0,

    /// <summary>
    /// Wholesale price for bulk purchases.
    /// </summary>
    Wholesale = 1,

    /// <summary>
    /// Promotional or discounted price.
    /// </summary>
    Promotion = 2,

    /// <summary>
    /// Special member or loyalty price.
    /// </summary>
    Member = 3
}
