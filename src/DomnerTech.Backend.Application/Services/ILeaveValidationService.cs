using DomnerTech.Backend.Domain.Entities;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Services;

/// <summary>
/// Service for leave request validation and business logic.
/// </summary>
public interface ILeaveValidationService : IBaseService
{
    /// <summary>
    /// Validates a leave request against policies and rules.
    /// </summary>
    Task<(bool IsValid, Dictionary<string, string[]> errors)> ValidateLeaveRequestAsync(
        ObjectId employeeId,
        ObjectId leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        decimal requestedDays,
        ObjectId? excludeRequestId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates leave days based on policy rules.
    /// </summary>
    Task<decimal> CalculateLeaveDaysAsync(
        ObjectId leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        bool isHalfDay = false,
        CancellationToken cancellationToken = default);
}
