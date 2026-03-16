namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the type of stock movement.
/// </summary>
public enum StockMovementType
{
    /// <summary>
    /// Initial stock entry into warehouse.
    /// </summary>
    InitialStock = 0,

    /// <summary>
    /// Stock received from supplier/purchase.
    /// </summary>
    PurchaseReceipt = 1,

    /// <summary>
    /// Stock sold to customer.
    /// </summary>
    Sale = 2,

    /// <summary>
    /// Stock returned by customer.
    /// </summary>
    SaleReturn = 3,

    /// <summary>
    /// Stock returned to supplier.
    /// </summary>
    PurchaseReturn = 4,

    /// <summary>
    /// Stock transferred out to another warehouse.
    /// </summary>
    TransferOut = 5,

    /// <summary>
    /// Stock received from another warehouse.
    /// </summary>
    TransferIn = 6,

    /// <summary>
    /// Manual adjustment increase.
    /// </summary>
    AdjustmentIncrease = 7,

    /// <summary>
    /// Manual adjustment decrease.
    /// </summary>
    AdjustmentDecrease = 8,

    /// <summary>
    /// Stock damaged or expired.
    /// </summary>
    Damage = 9,

    /// <summary>
    /// Stock reserved for order.
    /// </summary>
    Reservation = 10,

    /// <summary>
    /// Stock reservation released.
    /// </summary>
    ReservationRelease = 11,

    /// <summary>
    /// Stock count adjustment.
    /// </summary>
    StockCount = 12
}
