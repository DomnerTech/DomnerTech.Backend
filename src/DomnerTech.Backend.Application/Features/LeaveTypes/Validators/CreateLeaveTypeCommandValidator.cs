using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Validators;

/// <summary>
/// Validator for CreateLeaveTypeCommand.
/// </summary>
public sealed class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    public CreateLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeNameReq)
            .MaximumLength(100);

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeCodeReq)
            .MaximumLength(20)
            .Matches("^[A-Z0-9_]+$")
            .WithMessage("Code must contain only uppercase letters, numbers, and underscores");

        RuleFor(x => x.Dto.YearlyAllowance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Yearly allowance must be non-negative");

        When(x => x.Dto.IsAccrualBased, () =>
        {
            RuleFor(x => x.Dto.MonthlyAccrualDays)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Monthly accrual days must be specified for accrual-based leave types");
        });

        RuleFor(x => x.Dto.MaxCarryForwardDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Max carry forward days must be non-negative");

        When(x => x.Dto.CarryForwardExpires, () =>
        {
            RuleFor(x => x.Dto.CarryForwardExpiryDate)
                .NotNull()
                .WithMessage("Carry forward expiry date must be specified");
        });
    }
}
