using DomnerTech.Backend.Application.Services.Integrations;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services.Integrations;

/// <summary>
/// Stub implementation for payroll integration.
/// TODO: Implement actual integration with payroll system.
/// </summary>
public sealed class PayrollIntegrationService(
    ILogger<PayrollIntegrationService> logger) : IPayrollIntegrationService
{
    public async Task DeductUnpaidLeaveAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, decimal days, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual payroll system API call
        logger.LogInformation(
            "Deducting {Days} unpaid leave days from employee {EmployeeId} payroll for period {StartDate} to {EndDate}",
            days, employeeId, startDate, endDate);
        
        await Task.CompletedTask;
    }

    public async Task RevertLeaveDeductionAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual payroll system API call
        logger.LogInformation(
            "Reverting leave deduction for employee {EmployeeId} for period {StartDate} to {EndDate}",
            employeeId, startDate, endDate);
        
        await Task.CompletedTask;
    }
}