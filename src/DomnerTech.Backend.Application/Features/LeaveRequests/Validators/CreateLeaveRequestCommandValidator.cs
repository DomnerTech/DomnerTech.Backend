using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Validators;

/// <summary>
/// Validator for CreateLeaveRequestCommand.
/// </summary>
public sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestCommandValidator()
    {
        RuleFor(x => x.Dto.LeaveTypeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.TypeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid leave type ID format");

        RuleFor(x => x.Dto.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.StartDateReq);

        RuleFor(x => x.Dto.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EndDateReq)
            .GreaterThanOrEqualTo(x => x.Dto.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Dto.RequestType)
            .IsInEnum()
            .WithMessage("Invalid request type");

        RuleFor(x => x.Dto.Reason)
            .MaximumLength(500)
            .WithMessage("Reason must not exceed 500 characters");

        RuleFor(x => x.Dto.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes must not exceed 1000 characters");
    }
}