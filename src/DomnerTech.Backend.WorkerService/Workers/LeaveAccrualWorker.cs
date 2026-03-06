using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.WorkerService.Workers;

/// <summary>
/// Background service for processing monthly leave accruals.
/// Runs on the 1st of each month.
/// </summary>
public sealed class LeaveAccrualWorker(
    ILogger<LeaveAccrualWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Leave Accrual Worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                
                // Run on the 1st of each month at 1 AM
                if (now is { Day: 1, Hour: 1 })
                {
                    logger.LogInformation("Processing monthly leave accruals at: {Time}", DateTimeOffset.Now);
                    
                    using var scope = serviceProvider.CreateScope();
                    var leaveBalanceRepo = scope.ServiceProvider.GetRequiredService<ILeaveBalanceRepo>();
                    var leaveTypeRepo = scope.ServiceProvider.GetRequiredService<ILeaveTypeRepo>();

                    var currentYear = now.Year;
                    var allBalances = await leaveBalanceRepo.GetAllByYearAsync(currentYear, stoppingToken);

                    foreach (var balance in allBalances)
                    {
                        var leaveType = await leaveTypeRepo.GetByIdAsync(balance.LeaveTypeId, stoppingToken);

                        if (leaveType is not { IsAccrualBased: true, MonthlyAccrualDays: not null }
                            || balance.LastAccrualDate?.Month == now.Month) continue;

                        // Check if already accrued this month
                        balance.Allowance.TotalAllowance += leaveType.MonthlyAccrualDays.Value;
                        balance.LastAccrualDate = now;
                        balance.UpdatedAt = now;

                        await leaveBalanceRepo.UpdateAsync(balance, stoppingToken);
                                
                        logger.LogInformation(
                            "Accrued {Days} days for employee {EmployeeId}, leave type {LeaveTypeId}",
                            leaveType.MonthlyAccrualDays.Value,
                            balance.EmployeeId,
                            balance.LeaveTypeId);
                    }

                    logger.LogInformation("Monthly leave accrual processing completed");
                    
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
                logger.LogError(ex, "Error in Leave Accrual Worker: {Error}", ex.Message);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("Leave Accrual Worker stopped at: {Time}", DateTimeOffset.Now);
    }
}
