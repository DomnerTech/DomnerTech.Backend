using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Validators;

/// <summary>
/// Validator for UpdateLeaveTypeCommand.
/// </summary>
public sealed class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeNameReq)
            .MaximumLength(100);

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
