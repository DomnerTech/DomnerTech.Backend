using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Validators;

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