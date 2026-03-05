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

/// <summary>
/// Validator for RejectLeaveCommand.
/// </summary>
public sealed class RejectLeaveCommandValidator : AbstractValidator<RejectLeaveCommand>
{
    public RejectLeaveCommandValidator()
    {
        RuleFor(x => x.Dto.LeaveRequestId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.RequestIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave request ID format");

        RuleFor(x => x.Dto.RejectionReason)
            .NotEmpty()
            .WithMessage("Rejection reason is required")
            .MaximumLength(500)
            .WithMessage("Rejection reason must not exceed 500 characters");

        RuleFor(x => x.Dto.Comments)
            .MaximumLength(500)
            .WithMessage("Comments must not exceed 500 characters");
    }
}

/// <summary>
/// Validator for GetLeaveApprovalHistoryQuery.
/// </summary>
public sealed class GetLeaveApprovalHistoryQueryValidator : AbstractValidator<GetLeaveApprovalHistoryQuery>
{
    public GetLeaveApprovalHistoryQueryValidator()
    {
        RuleFor(x => x.LeaveRequestId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.RequestIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave request ID format");
    }
}
