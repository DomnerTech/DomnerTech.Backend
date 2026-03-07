using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class LeaveValidationService(
    ILeaveTypeRepo leaveTypeRepo,
    ILeavePolicyRepo leavePolicyRepo,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveRequestRepo leaveRequestRepo,
    IEmployeeRepo employeeRepo,
    IHolidayService holidayService) : ILeaveValidationService
{
    public async Task<(bool IsValid, Dictionary<string, string[]> errors)> ValidateLeaveRequestAsync(
        ObjectId employeeId,
        ObjectId leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        decimal requestedDays,
        ObjectId? excludeRequestId = null,
        CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();

        // Get leave type
        var leaveType = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);
        if (leaveType == null)
        {
            errors.Add("leave_type", ["Leave type not found"]);
            return (false, errors);
        }

        // Get policy
        var policy = await leavePolicyRepo.GetByLeaveTypeIdAsync(leaveTypeId, cancellationToken)
                     ?? await leavePolicyRepo.GetDefaultPolicyAsync(cancellationToken);

        if (policy == null)
        {
            errors.Add("policy", ["No policy found for this leave type"]);
            return (false, errors);
        }

        List<string> startDateErrors = [];
        // Validate dates
        if (startDate.Date < DateTime.UtcNow.Date && !policy.AllowBackdatedRequests)
        {
            startDateErrors.Add("Backdated leave requests are not allowed");
        }

        if (startDate.Date < DateTime.UtcNow.Date && policy.AllowBackdatedRequests)
        {
            var daysDiff = (DateTime.UtcNow.Date - startDate.Date).Days;
            if (policy.MaxBackdatedDays.HasValue && daysDiff > policy.MaxBackdatedDays.Value)
            {
                startDateErrors.Add($"Cannot request leave more than {policy.MaxBackdatedDays.Value} days in the past");
            }
        }

        if (startDateErrors.Count > 0)
        {
            errors.Add("start_date", [.. startDateErrors]);
        }

        if (endDate < startDate)
        {
            errors.Add("end_date", ["End date must be after start date"]);
        }

        // Validate minimum notice
        var noticeDays = (startDate.Date - DateTime.UtcNow.Date).Days;
        if (noticeDays < policy.MinimumNoticeDays)
        {
            errors.Add("notice_days", [$"Minimum notice period is {policy.MinimumNoticeDays} days"]);
        }

        // Validate maximum consecutive days
        var totalDays = (endDate.Date - startDate.Date).Days + 1;
        if (totalDays > policy.MaxConsecutiveDays)
        {
            errors.Add("total_days", [$"Maximum consecutive days allowed is {policy.MaxConsecutiveDays}"]);
        }

        // Check for overlapping requests
        if (await leaveRequestRepo.HasOverlappingRequestAsync(employeeId, startDate.Date, endDate.Date, excludeRequestId, cancellationToken))
        {
            errors.Add("overlap", ["You have an overlapping leave request"]);
        }

        // Validate balance
        var year = startDate.Year;
        var balance = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year, cancellationToken);
        
        if (balance == null)
        {
            errors.Add("balance", ["Leave balance not initialized for this leave type"]);
            return (false, errors);
        }

        var available = balance.Allowance.TotalAllowance + balance.Allowance.CarriedForwardDays - balance.Allowance.UsedDays;
        if (available < requestedDays && !policy.AllowNegativeBalance)
        {
            errors.Add("avl", [$"Insufficient leave balance. Available: {available} days, Requested: {requestedDays} days"]);
        }

        if (policy is { AllowNegativeBalance: true, MaxNegativeBalance: not null })
        {
            var negativeAmount = requestedDays - available;
            if (negativeAmount > policy.MaxNegativeBalance.Value)
            {
                errors.Add("negative_amt", [$"Negative balance limit exceeded. Maximum allowed: {policy.MaxNegativeBalance.Value} days"]);
            }
        }

        // Validate probation
        if (policy is { AllowDuringProbation: false, ProbationPeriodMonths: not null })
        {
            var employee = await employeeRepo.GetByIdAsync(employeeId, cancellationToken);
            if (employee != null)
            {
                var monthsSinceHire = (DateTime.UtcNow - employee.HireDate).Days / 30;
                if (monthsSinceHire < policy.ProbationPeriodMonths.Value)
                {
                    errors.Add("probation", [$"Cannot apply for leave during probation period ({policy.ProbationPeriodMonths.Value} months)"]);
                }
            }
        }

        // Validate document requirement
        if (leaveType.RequiresDocument && requestedDays > 3)
        {
            // Note: Document validation should be done in the handler where we have access to DocumentUrls
        }

        return (errors.Count == 0, errors);
    }

    public async Task<decimal> CalculateLeaveDaysAsync(
        ObjectId leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        bool isHalfDay = false,
        CancellationToken cancellationToken = default)
    {
        var policy = await leavePolicyRepo.GetByLeaveTypeIdAsync(leaveTypeId, cancellationToken)
                     ?? await leavePolicyRepo.GetDefaultPolicyAsync(cancellationToken);

        if (policy == null)
        {
            // Default calculation
            var days = (endDate.Date - startDate.Date).Days + 1;
            return isHalfDay ? 0.5m : days;
        }

        var workingDays = await holidayService.CalculateWorkingDaysAsync(
            startDate.Date,
            endDate.Date,
            policy.IncludeWeekends,
            cancellationToken);

        return isHalfDay ? 0.5m : workingDays;
    }
}
