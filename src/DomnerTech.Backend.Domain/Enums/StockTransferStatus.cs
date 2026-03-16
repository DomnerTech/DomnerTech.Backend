namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the status of a stock transfer between warehouses.
/// </summary>
public enum StockTransferStatus
{
    /// <summary>
    /// Transfer request is pending approval.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Transfer is approved and ready for shipment.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Transfer is in transit.
    /// </summary>
    InTransit = 2,

    /// <summary>
    /// Transfer is completed and stock received.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Transfer was cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Transfer was rejected.
    /// </summary>
    Rejected = 5
}
