using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Validators;

/// <summary>
/// Validator for GetLeaveRequestByIdQuery.
/// </summary>
public sealed class GetLeaveRequestByIdQueryValidator : AbstractValidator<GetLeaveRequestByIdQuery>
{
    public GetLeaveRequestByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.RequestIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave request ID format");
    }
}
