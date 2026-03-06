using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Validators;

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