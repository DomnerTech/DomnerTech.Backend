using System.Text.Json.Serialization;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Domain.Entities;

namespace DomnerTech.Backend.Application.DTOs.Employees;

/// <summary>
/// Data transfer object for employee information.
/// </summary>
public class EmployeeDto : IBaseDto
{
    public required string Id { get; set; }
    public required string CompanyId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public required string Department { get; set; }
    public required string JobTitle { get; set; }
    public AddressDto? Address { get; set; }
    public required string EmployeeNumber { get; set; }
    [JsonIgnore]
    public DateTime CreatedAt { get; set; }
    [JsonIgnore]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Extension methods for employee entity and DTO conversions.
/// </summary>
public static class EmployeeExtensions
{
    /// <summary>
    /// Converts an employee DTO to an entity.
    /// </summary>
    /// <param name="dto">The employee DTO to convert.</param>
    /// <returns>The converted employee entity.</returns>
    public static EmployeeEntity ToEntity(this EmployeeDto dto) => new()
    {
        Id = dto.Id.ToObjectId(),
        CompanyId = dto.CompanyId.ToObjectId(),
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        DateOfBirth = dto.DateOfBirth,
        HireDate = dto.HireDate,
        IsActive = dto.IsActive,
        Department = dto.Department,
        JobTitle = dto.JobTitle,
        Address = dto.Address?.ToValueObject(),
        EmployeeNumber = dto.EmployeeNumber,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt
    };

    /// <summary>
    /// Converts an employee entity to a DTO.
    /// </summary>
    /// <param name="entity">The employee entity to convert.</param>
    /// <returns>The converted employee DTO.</returns>
    public static EmployeeDto ToDto(this EmployeeEntity entity) => new()
    {
        Id = entity.Id.ToString(),
        CompanyId = entity.CompanyId.ToString(),
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email,
        PhoneNumber = entity.PhoneNumber,
        DateOfBirth = entity.DateOfBirth,
        HireDate = entity.HireDate,
        IsActive = entity.IsActive,
        Department = entity.Department,
        JobTitle = entity.JobTitle,
        Address = entity.Address?.ToDto(),
        EmployeeNumber = entity.EmployeeNumber,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}
