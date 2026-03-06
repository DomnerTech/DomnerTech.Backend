using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Validators;

/// <summary>
/// Validator for GetTeamLeaveCalendarQuery.
/// </summary>
public sealed class GetTeamLeaveCalendarQueryValidator : AbstractValidator<GetTeamLeaveCalendarQuery>
{
    public GetTeamLeaveCalendarQueryValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.DepartmentReq)
            .WithMessage("Department is required");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.StartDateReq)
            .WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EndDateReq)
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date");
    }
}
