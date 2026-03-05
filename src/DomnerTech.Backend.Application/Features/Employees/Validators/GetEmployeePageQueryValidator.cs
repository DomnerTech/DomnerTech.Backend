using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Employees.Validators;

/// <summary>
/// Validator for GetEmployeePageQuery to ensure valid pagination parameters.
/// </summary>
public sealed class GetEmployeePageQueryValidator : AbstractValidator<GetEmployeePageQuery>
{
    public GetEmployeePageQueryValidator()
    {
        RuleFor(i => i.PageSize)
            .LessThanOrEqualTo(100)
            .WithErrorCode(ErrorCodes.MaxPageSize);
    }
}
