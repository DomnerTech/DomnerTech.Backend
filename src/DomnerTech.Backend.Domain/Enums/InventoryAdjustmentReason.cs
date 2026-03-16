namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the reason for inventory adjustment.
/// </summary>
public enum InventoryAdjustmentReason
{
    /// <summary>
    /// Stock count discrepancy found during physical counting.
    /// </summary>
    StockCount = 0,

    /// <summary>
    /// Damaged goods.
    /// </summary>
    Damage = 1,

    /// <summary>
    /// Expired products.
    /// </summary>
    Expiry = 2,

    /// <summary>
    /// Lost or stolen items.
    /// </summary>
    Loss = 3,

    /// <summary>
    /// Found additional stock.
    /// </summary>
    Found = 4,

    /// <summary>
    /// System correction or data migration.
    /// </summary>
    SystemCorrection = 5,

    /// <summary>
    /// Other reason with notes.
    /// </summary>
    Other = 6
}
