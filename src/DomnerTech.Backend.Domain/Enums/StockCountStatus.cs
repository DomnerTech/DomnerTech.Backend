namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the status of a stock counting process.
/// </summary>
public enum StockCountStatus
{
    /// <summary>
    /// Stock count is planned but not started.
    /// </summary>
    Planned = 0,

    /// <summary>
    /// Stock count is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Stock count is completed and pending review.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Stock count is reviewed and adjustments applied.
    /// </summary>
    Approved = 3,

    /// <summary>
    /// Stock count was cancelled.
    /// </summary>
    Cancelled = 4
}
