namespace DomnerTech.Backend.Application.DTOs.Employees;

/// <summary>
/// Request DTO for updating an employee's information.
/// </summary>
public sealed class UpdateEmployeeReqDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the employee to update.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's date of birth.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the employee's department.
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's job title.
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's address information.
    /// </summary>
    public AddressDto? Address { get; set; }

    /// <summary>
    /// Gets or sets the employee's active status.
    /// </summary>
    public bool IsActive { get; set; }
}
