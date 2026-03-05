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

public sealed class UpdateLeavePolicyCommandValidator : AbstractValidator<UpdateLeavePolicyCommand>
{
    public UpdateLeavePolicyCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Policy ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid policy ID format");

        RuleFor(x => x.Dto.PolicyName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.PolicyNameReq)
            .MaximumLength(100);

        RuleFor(x => x.Dto.MinimumNoticeDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum notice days must be non-negative");

        RuleFor(x => x.Dto.MaxConsecutiveDays)
            .GreaterThan(0)
            .WithMessage("Maximum consecutive days must be positive");
    }
}

public sealed class DeleteLeavePolicyCommandValidator : AbstractValidator<DeleteLeavePolicyCommand>
{
    public DeleteLeavePolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Policy ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid policy ID format");
    }
}

public sealed class GetLeavePolicyByIdQueryValidator : AbstractValidator<GetLeavePolicyByIdQuery>
{
    public GetLeavePolicyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Policy ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid policy ID format");
    }
}

public sealed class GetPolicyByLeaveTypeQueryValidator : AbstractValidator<GetPolicyByLeaveTypeQuery>
{
    public GetPolicyByLeaveTypeQueryValidator()
    {
        RuleFor(x => x.LeaveTypeId)
            .NotEmpty()
            .WithMessage("Leave type ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");
    }
}
