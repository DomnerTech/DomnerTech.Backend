using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;

namespace DomnerTech.Backend.WorkerService.Workers;

/// <summary>
/// Background service for sending leave-related notifications.
/// Sends reminders and notifications about pending leave requests, upcoming leave, etc.
/// </summary>
public sealed class LeaveNotificationWorker(
    ILogger<LeaveNotificationWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Leave Notification Worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                
                // Run every 6 hours
                logger.LogInformation("Processing leave notifications at: {Time}", DateTimeOffset.Now);
                
                using var scope = serviceProvider.CreateScope();
                var leaveRequestRepo = scope.ServiceProvider.GetRequiredService<ILeaveRequestRepo>();
                var leaveBalanceRepo = scope.ServiceProvider.GetRequiredService<ILeaveBalanceRepo>();

                // 1. Check for pending approvals (older than 2 days)
                var pendingRequests = await leaveRequestRepo.GetPendingApprovalsAsync(stoppingToken);
                var oldPendingRequests = pendingRequests
                    .Where(r => (now - r.SubmittedAt).TotalDays > 2)
                    .ToList();

                if (oldPendingRequests.Count > 0)
                {
                    logger.LogInformation(
                        "Found {Count} pending leave requests older than 2 days requiring attention",
                        oldPendingRequests.Count);
                    
                    // TODO: Send email notifications to approvers
                    foreach (var request in oldPendingRequests)
                    {
                        logger.LogInformation(
                            "Reminder needed for leave request {RequestId} submitted by employee {EmployeeId}",
                            request.Id,
                            request.EmployeeId);
                    }
                }

                // 2. Check for upcoming approved leave (starting in 3 days)
                var upcomingDate = now.AddDays(3).Date;
                var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, stoppingToken);
                var upcomingLeave = approvedRequests
                    .Where(r => r.Period.StartDate.Date == upcomingDate)
                    .ToList();

                if (upcomingLeave.Count > 0)
                {
                    logger.LogInformation(
                        "Found {Count} employees with leave starting in 3 days",
                        upcomingLeave.Count);
                    
                    // TODO: Send reminders to employees and their managers
                    foreach (var request in upcomingLeave)
                    {
                        logger.LogInformation(
                            "Upcoming leave reminder for employee {EmployeeId} starting on {StartDate}",
                            request.EmployeeId,
                            request.Period.StartDate);
                    }
                }

                // 3. Check for low leave balances (less than 5 days remaining)
                var currentYear = now.Year;
                var allBalances = await leaveBalanceRepo.GetAllByYearAsync(currentYear, stoppingToken);
                var lowBalances = allBalances
                    .Where(b => b.Allowance.RemainingDays < 5 && b.Allowance.RemainingDays > 0)
                    .ToList();

                if (lowBalances.Count > 0)
                {
                    logger.LogInformation(
                        "Found {Count} employees with low leave balances",
                        lowBalances.Count);
                    
                    // TODO: Send notification to employees about low balance
                    foreach (var balance in lowBalances)
                    {
                        logger.LogInformation(
                            "Low balance notification for employee {EmployeeId}: {Remaining} days remaining",
                            balance.EmployeeId,
                            balance.Allowance.RemainingDays);
                    }
                }

                // 4. Check for expiring carry forward leave (within 30 days)
                var expiringCarryForward = allBalances
                    .Where(b => b.CarryForwardExpiryDate.HasValue &&
                                b.CarryForwardExpiryDate.Value <= now.AddDays(30) &&
                                b.Allowance.CarriedForwardDays > 0)
                    .ToList();

                if (expiringCarryForward.Count > 0)
                {
                    logger.LogInformation(
                        "Found {Count} employees with expiring carry forward leave",
                        expiringCarryForward.Count);
                    
                    // TODO: Send notification about expiring carried forward days
                    foreach (var balance in expiringCarryForward)
                    {
                        logger.LogInformation(
                            "Expiring carry forward notification for employee {EmployeeId}: {Days} days expiring on {ExpiryDate}",
                            balance.EmployeeId,
                            balance.Allowance.CarriedForwardDays,
                            balance.CarryForwardExpiryDate);
                    }
                }

                logger.LogInformation("Leave notification processing completed");
                
                // Wait 6 hours before next run
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Leave Notification Worker: {Error}", ex.Message);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("Leave Notification Worker stopped at: {Time}", DateTimeOffset.Now);
    }
}
