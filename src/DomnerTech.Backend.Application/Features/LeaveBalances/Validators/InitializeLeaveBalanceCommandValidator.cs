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