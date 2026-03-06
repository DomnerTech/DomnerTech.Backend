using MongoDB.Bson;

namespace DomnerTech.Backend.Application.DTOs;

public class LogActionParams
{
    public ObjectId UserId { get; set; }
    public required string Username { get; set; }
    public required string Action { get; set; }
    public required string EntityType { get; set; }
    public ObjectId? EntityId { get; set; }
    public required string Description { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
}