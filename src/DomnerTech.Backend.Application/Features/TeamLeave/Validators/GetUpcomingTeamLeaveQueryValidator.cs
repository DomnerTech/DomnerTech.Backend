using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Validators;

/// <summary>
/// Validator for GetUpcomingTeamLeaveQuery.
/// </summary>
public sealed class GetUpcomingTeamLeaveQueryValidator : AbstractValidator<GetUpcomingTeamLeaveQuery>
{
    public GetUpcomingTeamLeaveQueryValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.DepartmentReq)
            .WithMessage("Department is required");
    }
}
