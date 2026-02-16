using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

[MongoCollection("roles")]
public sealed class RoleEntity : IBaseEntity
{
    public required ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string? Desc { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}