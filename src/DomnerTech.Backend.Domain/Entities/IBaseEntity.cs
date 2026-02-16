using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

public interface IBaseEntity
{
    ObjectId Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}