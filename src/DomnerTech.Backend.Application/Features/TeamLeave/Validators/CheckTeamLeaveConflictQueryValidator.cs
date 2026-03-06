using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Validators;

/// <summary>
/// Validator for CheckTeamLeaveConflictQuery.
/// </summary>
public sealed class CheckTeamLeaveConflictQueryValidator : AbstractValidator<CheckTeamLeaveConflictQuery>
{
    public CheckTeamLeaveConflictQueryValidator()
    {
        RuleFor(x => x.Dto.Department)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.DepartmentReq)
            .WithMessage("Department is required");

        RuleFor(x => x.Dto.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.StartDateReq)
            .WithMessage("Start date is required");

        RuleFor(x => x.Dto.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EndDateReq)
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.Dto.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Dto.MaxEmployeesOnLeave)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodes.Leave.MaxEmployeesReq)
            .WithMessage("Max employees on leave must be greater than 0");

        When(x => !string.IsNullOrWhiteSpace(x.Dto.ExcludeEmployeeId), () =>
        {
            RuleFor(x => x.Dto.ExcludeEmployeeId)
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid employee ID format");
        });
    }
}
