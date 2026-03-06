using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Validators;

public sealed class CreateLeavePolicyCommandValidator : AbstractValidator<CreateLeavePolicyCommand>
{
    public CreateLeavePolicyCommandValidator()
    {
        RuleFor(x => x.Dto.PolicyName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.PolicyNameReq)
            .MaximumLength(100);

        When(x => !string.IsNullOrEmpty(x.Dto.LeaveTypeId), () =>
        {
            RuleFor(x => x.Dto.LeaveTypeId)
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid leave type ID format");
        });

        RuleFor(x => x.Dto.MinimumNoticeDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum notice days must be non-negative");

        RuleFor(x => x.Dto.MaxConsecutiveDays)
            .GreaterThan(0)
            .WithMessage("Maximum consecutive days must be positive");

        RuleFor(x => x.Dto.EffectiveFrom)
            .NotEmpty()
            .WithMessage("Effective from date is required");

        When(x => x.Dto.AllowNegativeBalance, () =>
        {
            RuleFor(x => x.Dto.MaxNegativeBalance)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Max negative balance must be specified when negative balance is allowed");
        });

        When(x => x.Dto.AllowBackdatedRequests, () =>
        {
            RuleFor(x => x.Dto.MaxBackdatedDays)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Max backdated days must be specified when backdated requests are allowed");
        });

        When(x => !x.Dto.AllowDuringProbation, () =>
        {
            RuleFor(x => x.Dto.ProbationPeriodMonths)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Probation period must be specified");
        });
    }
}