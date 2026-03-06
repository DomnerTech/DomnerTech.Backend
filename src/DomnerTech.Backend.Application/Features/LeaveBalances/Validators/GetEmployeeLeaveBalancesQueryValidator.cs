using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Validators;

/// <summary>
/// Validator for GetEmployeeLeaveBalancesQuery.
/// </summary>
public sealed class GetEmployeeLeaveBalancesQueryValidator : AbstractValidator<GetEmployeeLeaveBalancesQuery>
{
    public GetEmployeeLeaveBalancesQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EmployeeIdReq)
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid employee ID format");

        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be less than or equal to 2100");
    }
}
