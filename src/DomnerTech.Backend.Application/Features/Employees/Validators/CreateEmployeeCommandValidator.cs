using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Employees.Validators;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        // ==============================
        // Date of Birth
        // ==============================

        RuleFor(x => x.Dto.DateOfBirth)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.DobReq)
            .Must(BeBetween18And65)
            .WithErrorCode(ErrorCodes.Employee.DobInvalidAge);

        // ==============================
        // Department
        // ==============================

        RuleFor(x => x.Dto.Department)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.DepartmentReq);

        // ==============================
        // Email
        // ==============================

        RuleFor(x => x.Dto.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.EmailReq)
            .EmailAddress()
            .WithErrorCode(ErrorCodes.Employee.EmailInvalid);

        // ==============================
        // First Name
        // ==============================

        RuleFor(x => x.Dto.FirstName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.FirstNameReq);

        // ==============================
        // Last Name
        // ==============================

        RuleFor(x => x.Dto.LastName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.LastNameReq);

        // ==============================
        // Job Title
        // ==============================

        RuleFor(x => x.Dto.JobTitle)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.JobTitleReq);

        // ==============================
        // Phone Number
        // ==============================

        RuleFor(x => x.Dto.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.PhoneNumberReq);

        // ==============================
        // Hire Date
        // ==============================

        RuleFor(x => x.Dto.HireDate)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Employee.HireDateReq)
            .Must(date => date.Date <= DateTime.UtcNow.Date)
            .WithErrorCode(ErrorCodes.Employee.HireDateFuture);

        // ==============================
        // Address Object
        // ==============================

        RuleFor(x => x.Dto.Address)
            .NotNull()
            .WithErrorCode(ErrorCodes.Employee.AddressReq);

        When(x => x.Dto.Address != null, () =>
        {
            RuleFor(x => x.Dto.Address!.Street)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Employee.StreetReq);

            RuleFor(x => x.Dto.Address!.City)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Employee.CityReq);

            RuleFor(x => x.Dto.Address!.PostalCode)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Employee.PostalCodeReq);

            RuleFor(x => x.Dto.Address!.Country)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Employee.CountryReq);
        });
    }

    // ============================================
    // Private Business Rule Methods
    // ============================================

    private static bool BeBetween18And65(DateTime dob)
    {
        var today = DateTime.UtcNow.Date;

        var minDob = today.AddYears(-65);
        var maxDob = today.AddYears(-18);

        var birth = dob.Date;

        return birth >= minDob && birth <= maxDob;
    }
}