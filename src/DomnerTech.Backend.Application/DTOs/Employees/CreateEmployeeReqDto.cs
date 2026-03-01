namespace DomnerTech.Backend.Application.DTOs.Employees;

public sealed class CreateEmployeeReqDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }

    public string Department { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
}