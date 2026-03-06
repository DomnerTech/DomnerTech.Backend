using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Validators;

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
