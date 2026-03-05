using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Validators;

/// <summary>
/// Validator for InitializeLeaveBalanceCommand.
/// </summary>
public sealed class InitializeLeaveBalanceCommandValidator : AbstractValidator<InitializeLeaveBalanceCommand>
{
    public InitializeLeaveBalanceCommandValidator()
    {
        RuleFor(x => x.Dto.EmployeeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EmployeeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid employee ID format");

        RuleFor(x => x.Dto.LeaveTypeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");

        RuleFor(x => x.Dto.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be less than or equal to 2100");

        RuleFor(x => x.Dto.TotalAllowance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total allowance must be non-negative");

        RuleFor(x => x.Dto.CarriedForwardDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Carried forward days must be non-negative");
    }
}

/// <summary>
/// Validator for AdjustLeaveBalanceCommand.
/// </summary>
public sealed class AdjustLeaveBalanceCommandValidator : AbstractValidator<AdjustLeaveBalanceCommand>
{
    public AdjustLeaveBalanceCommandValidator()
    {
        RuleFor(x => x.Dto.EmployeeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EmployeeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid employee ID format");

        RuleFor(x => x.Dto.LeaveTypeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");

        RuleFor(x => x.Dto.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be less than or equal to 2100");

        RuleFor(x => x.Dto.AdjustmentDays)
            .NotEqual(0)
            .WithMessage("Adjustment days cannot be zero");

        RuleFor(x => x.Dto.Reason)
            .NotEmpty()
            .WithMessage("Reason for adjustment is required")
            .MaximumLength(500)
            .WithMessage("Reason must not exceed 500 characters");
    }
}

/// <summary>
/// Validator for GetEmployeeLeaveBalancesQuery.
/// </summary>
public sealed class GetEmployeeLeaveBalancesQueryValidator : AbstractValidator<GetEmployeeLeaveBalancesQuery>
{
    public GetEmployeeLeaveBalancesQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EmployeeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid employee ID format");

        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be less than or equal to 2100");
    }
}
