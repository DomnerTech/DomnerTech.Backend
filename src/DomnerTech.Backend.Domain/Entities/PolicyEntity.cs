using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

[MongoCollection("policies")]
public class PolicyEntity : IBaseEntity
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string? Desc { get; set; }
    public required HashSet<string> RequiredRoles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}