using MongoDB.Bson;

namespace DomnerTech.Backend.Domain.Entities;

public interface ITenantEntity
{
    ObjectId CompanyId { get; set; }
}