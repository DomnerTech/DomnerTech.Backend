using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

[MongoCollection("users")]
public class UserEntity : IBaseEntity, IAuditEntity, ISoftDeleteEntity, ITenantEntity
{
    public required ObjectId Id { get; set; }
    public required ObjectId EmpId { get; set; }
    public required ObjectId CompanyId { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required HashSet<string> Policies { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ObjectId? UpdatedBy { get; set; }
    public ObjectId? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}