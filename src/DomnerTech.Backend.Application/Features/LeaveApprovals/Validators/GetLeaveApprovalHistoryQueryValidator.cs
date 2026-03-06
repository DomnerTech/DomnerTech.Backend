using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Validators;

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
