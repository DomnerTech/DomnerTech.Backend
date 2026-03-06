using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Validators;

/// <summary>
/// Validator for ApproveLeaveCommand.
/// </summary>
public sealed class ApproveLeaveCommandValidator : AbstractValidator<ApproveLeaveCommand>
{
    public ApproveLeaveCommandValidator()
    {
        RuleFor(x => x.Dto.LeaveRequestId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.RequestIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave request ID format");

        RuleFor(x => x.Dto.Comments)
            .MaximumLength(500)
            .WithMessage("Comments must not exceed 500 characters");
    }
}