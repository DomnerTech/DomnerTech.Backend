using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Validators;

/// <summary>
/// Validator for DeleteLeaveTypeCommand.
/// </summary>
public sealed class DeleteLeaveTypeCommandValidator : AbstractValidator<DeleteLeaveTypeCommand>
{
    public DeleteLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");
    }
}
