using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.WorkerService.Workers;

/// <summary>
/// Background service for handling expired carried forward leave.
/// Checks daily for expired carry forward leave and removes it from balances.
/// </summary>
public sealed class LeaveExpiryWorker(
    ILogger<LeaveExpiryWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Leave Expiry Worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                
                // Run daily at 3 AM
                if (now.Hour == 3)
                {
                    logger.LogInformation("Processing expired carry forward leave at: {Time}", DateTimeOffset.Now);
                    
                    using var scope = serviceProvider.CreateScope();
                    var leaveBalanceRepo = scope.ServiceProvider.GetRequiredService<ILeaveBalanceRepo>();
                    var leaveTypeRepo = scope.ServiceProvider.GetRequiredService<ILeaveTypeRepo>();

                    var currentYear = now.Year;
                    var allBalances = await leaveBalanceRepo.GetAllByYearAsync(currentYear, stoppingToken);

                    foreach (var balance in allBalances)
                    {
                        if (!balance.CarryForwardExpiryDate.HasValue ||
                            balance.CarryForwardExpiryDate.Value.Date > now.Date ||
                            balance.Allowance.CarriedForwardDays <= 0) continue;

                        var leaveType = await leaveTypeRepo.GetByIdAsync(balance.LeaveTypeId, stoppingToken);

                        if (leaveType?.CarryForwardExpires != true) continue;
                        logger.LogInformation(
                            "Expiring {Days} carried forward days for employee {EmployeeId}, leave type {LeaveTypeId}",
                            balance.Allowance.CarriedForwardDays,
                            balance.EmployeeId,
                            balance.LeaveTypeId);

                        balance.Allowance.CarriedForwardDays = 0;
                        balance.CarryForwardExpiryDate = null;
                        balance.UpdatedAt = now;
                                
                        await leaveBalanceRepo.UpdateAsync(balance, stoppingToken);
                    }

                    logger.LogInformation("Leave expiry processing completed");
                    
                    // Wait 1 hour to avoid reprocessing
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
                else
                {
                    // Check every hour
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Leave Expiry Worker: {Error}", ex.Message);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("Leave Expiry Worker stopped at: {Time}", DateTimeOffset.Now);
    }
}
