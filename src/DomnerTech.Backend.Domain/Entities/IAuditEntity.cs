using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

public interface IAuditEntity
{
    ObjectId? UpdatedBy { get; set; }
    ObjectId? DeletedBy { get; set; }
}