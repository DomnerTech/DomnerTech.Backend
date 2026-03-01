using MongoDB.Bson;
using DomnerTech.Backend.Domain.ValueObjects;

namespace DomnerTech.Backend.Domain.Entities;

[MongoCollection("employees")]
public class EmployeeEntity : IBaseEntity, ITenantEntity, IAuditEntity
{
    [Sortable(alias: "id", order: 1)]
    public ObjectId Id { get; set; }
    public ObjectId CompanyId { get; set; }

    // Employee Information
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Employment Details
    public required string Department { get; set; }
    public required string JobTitle { get; set; }
    public required AddressValueObject Address { get; set; }
    public required string EmployeeNumber { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
}