using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.TeamLeave.Validators;

/// <summary>
/// Validator for GetTeamLeaveStatsQuery.
/// </summary>
public sealed class GetTeamLeaveStatsQueryValidator : AbstractValidator<GetTeamLeaveStatsQuery>
{
    public GetTeamLeaveStatsQueryValidator()
    {
        RuleFor(x => x.Department)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.DepartmentReq)
            .WithMessage("Department is required");
    }
}
