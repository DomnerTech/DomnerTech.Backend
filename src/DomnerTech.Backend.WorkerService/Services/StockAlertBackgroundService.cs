using DomnerTech.Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.WorkerService.Services;

/// <summary>
/// Background service that monitors stock levels and sends alerts for low stock items.
/// Runs every hour to check for products below reorder level.
/// </summary>
public sealed class StockAlertBackgroundService : BackgroundService
{
    private readonly ILogger<StockAlertBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Run every hour

    public StockAlertBackgroundService(
        ILogger<StockAlertBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stock Alert Background Service is starting");

        // Wait a bit before starting (allow application to fully initialize)
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Stock Alert Background Service is running at: {Time}", DateTimeOffset.UtcNow);

                await CheckStockAlertsAsync(stoppingToken);
                await CheckExpiryAlertsAsync(stoppingToken);
                await ProcessExpiredReservationsAsync(stoppingToken);

                _logger.LogInformation("Stock Alert Background Service completed check at: {Time}", DateTimeOffset.UtcNow);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Stock Alert Background Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Stock Alert Background Service: {Error}", ex.Message);
            }

            // Wait for the next interval
            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Stock Alert Background Service has stopped");
    }

    /// <summary>
    /// Checks for low stock items and sends alerts.
    /// </summary>
    private async Task CheckStockAlertsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var stockAlertService = scope.ServiceProvider.GetRequiredService<IStockAlertService>();

            _logger.LogInformation("Checking for low stock items...");
            await stockAlertService.CheckLowStockAlertsAsync(cancellationToken);
            _logger.LogInformation("Low stock check completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking low stock alerts: {Error}", ex.Message);
        }
    }

    /// <summary>
    /// Checks for products nearing expiry and sends alerts.
    /// </summary>
    private async Task CheckExpiryAlertsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var stockAlertService = scope.ServiceProvider.GetRequiredService<IStockAlertService>();

            _logger.LogInformation("Checking for expiring products...");
            await stockAlertService.CheckExpiryAlertsAsync(cancellationToken);
            _logger.LogInformation("Expiry check completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking expiry alerts: {Error}", ex.Message);
        }
    }

    /// <summary>
    /// Processes and releases expired stock reservations.
    /// </summary>
    private async Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();

            _logger.LogInformation("Processing expired reservations...");
            await inventoryService.ProcessExpiredReservationsAsync(cancellationToken);
            _logger.LogInformation("Expired reservations processed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing expired reservations: {Error}", ex.Message);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock Alert Background Service is stopping");
        await base.StopAsync(cancellationToken);
    }
}
