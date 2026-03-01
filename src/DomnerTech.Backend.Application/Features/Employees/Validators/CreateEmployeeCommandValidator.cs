using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Employees.Validators;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(i => i.Dto.DateOfBirth)
            .NotNull()
            .Must(dob => {
                var today = DateTime.UtcNow.Date;
                var minDob = today.AddYears(-65);
                var maxDob = today.AddYears(-18);

                var birth = dob.Date;

                return birth >= minDob && birth <= maxDob;
            })
            .WithMessage("Age must be between 18 and 65 years.");

        RuleFor(i => i.Dto.Department)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.Email)
            .NotNull()
            .EmailAddress();

        RuleFor(i => i.Dto.FirstName)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.HireDate)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.JobTitle)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.LastName)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.PhoneNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Dto.Address.Street)
            .NotNull()
            .NotEmpty();
        RuleFor(i => i.Dto.Address.City)
            .NotNull()
            .NotEmpty();
        RuleFor(i => i.Dto.Address.PostalCode)
            .NotNull()
            .NotEmpty();
        RuleFor(i => i.Dto.Address.Country)
            .NotNull()
            .NotEmpty();
    }
}