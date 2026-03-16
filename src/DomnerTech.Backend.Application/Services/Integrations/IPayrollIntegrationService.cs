using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services.Integrations;

/// <summary>
/// Interface for payroll system integration.
/// </summary>
public interface IPayrollIntegrationService : IBaseService
{
    /// <summary>
    /// Notifies payroll system about unpaid leave deduction.
    /// </summary>
    Task DeductUnpaidLeaveAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, decimal days, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverts unpaid leave deduction when leave is cancelled.
    /// </summary>
    Task RevertLeaveDeductionAsync(ObjectId employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}