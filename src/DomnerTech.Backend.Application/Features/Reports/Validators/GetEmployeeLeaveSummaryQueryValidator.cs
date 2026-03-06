using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Reports.Validators;

/// <summary>
/// Validator for GetEmployeeLeaveSummaryQuery.
/// </summary>
public sealed class GetEmployeeLeaveSummaryQueryValidator : AbstractValidator<GetEmployeeLeaveSummaryQuery>
{
    public GetEmployeeLeaveSummaryQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.EmployeeIdReq)
            .WithMessage("Employee ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid employee ID format");

        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("Year cannot be more than one year in the future");
    }
}
