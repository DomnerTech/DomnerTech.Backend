using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service interface for stock alert notifications.
/// </summary>
public interface IStockAlertService : IBaseService
{
    /// <summary>
    /// Checks all products for low stock and sends alerts.
    /// </summary>
    Task CheckLowStockAlertsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends low stock alert for a specific product.
    /// </summary>
    Task SendLowStockAlertAsync(StockEntity stock, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks for expired products and sends alerts.
    /// </summary>
    Task CheckExpiryAlertsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a stock alert report.
    /// </summary>
    Task<List<StockEntity>> GetLowStockItemsAsync(ObjectId? warehouseId = null, CancellationToken cancellationToken = default);
}
