using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

[MongoCollection("errorMessageLocalizes")]
public class ErrorMessageLocalizeEntity : IBaseEntity
{
    public required ObjectId Id { get; set; }
    public required string Key { get; set; }
    public required Dictionary<string, string> Messages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}