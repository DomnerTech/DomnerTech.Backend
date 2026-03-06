using DomnerTech.Backend.Application.IRepo;

namespace DomnerTech.Backend.WorkerService.Workers;

/// <summary>
/// Background service for processing end-of-year leave carry forwards.
/// Runs on January 1st to carry forward unused leave to the new year.
/// </summary>
public sealed class LeaveCarryForwardWorker(
    ILogger<LeaveCarryForwardWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Leave Carry Forward Worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                
                // Run on January 1st at 2 AM
                if (now is { Month: 1, Day: 1, Hour: 2 })
                {
                    logger.LogInformation("Processing leave carry forwards at: {Time}", DateTimeOffset.Now);
                    
                    using var scope = serviceProvider.CreateScope();
                    var leaveBalanceRepo = scope.ServiceProvider.GetRequiredService<ILeaveBalanceRepo>();
                    var leaveTypeRepo = scope.ServiceProvider.GetRequiredService<ILeaveTypeRepo>();

                    var previousYear = now.Year - 1;
                    var currentYear = now.Year;
                    
                    var previousYearBalances = await leaveBalanceRepo.GetAllByYearAsync(previousYear, stoppingToken);

                    foreach (var balance in previousYearBalances)
                    {
                        var leaveType = await leaveTypeRepo.GetByIdAsync(balance.LeaveTypeId, stoppingToken);
                        
                        if (leaveType == null) continue;

                        var remainingDays = balance.Allowance.RemainingDays;
                        var carriedForwardDays = Math.Min(remainingDays, leaveType.MaxCarryForwardDays);

                        if (carriedForwardDays > 0)
                        {
                            // Check if balance already exists for current year
                            var currentYearBalance = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(
                                balance.EmployeeId,
                                balance.LeaveTypeId,
                                currentYear,
                                stoppingToken);

                            if (currentYearBalance != null)
                            {
                                currentYearBalance.Allowance.CarriedForwardDays = carriedForwardDays;
                                
                                if (leaveType is { CarryForwardExpires: true, CarryForwardExpiryDate: not null })
                                {
                                    currentYearBalance.CarryForwardExpiryDate = new DateTime(
                                        currentYear,
                                        leaveType.CarryForwardExpiryDate.Value.Month,
                                        leaveType.CarryForwardExpiryDate.Value.Day);
                                }
                                
                                currentYearBalance.UpdatedAt = now;
                                await leaveBalanceRepo.UpdateAsync(currentYearBalance, stoppingToken);

                                logger.LogInformation(
                                    "Carried forward {Days} days for employee {EmployeeId}, leave type {LeaveTypeId}",
                                    carriedForwardDays,
                                    balance.EmployeeId,
                                    balance.LeaveTypeId);
                            }
                        }

                        // Deactivate previous year balance
                        balance.IsActive = false;
                        balance.UpdatedAt = now;
                        await leaveBalanceRepo.UpdateAsync(balance, stoppingToken);
                    }

                    logger.LogInformation("Leave carry forward processing completed");
                    
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
                logger.LogError(ex, "Error in Leave Carry Forward Worker: {Error}", ex.Message);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("Leave Carry Forward Worker stopped at: {Time}", DateTimeOffset.Now);
    }
}
