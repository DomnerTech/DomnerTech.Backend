using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Validators;

/// <summary>
/// Validator for GetLeaveTypeByIdQuery.
/// </summary>
public sealed class GetLeaveTypeByIdQueryValidator : AbstractValidator<GetLeaveTypeByIdQuery>
{
    public GetLeaveTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");
    }
}
