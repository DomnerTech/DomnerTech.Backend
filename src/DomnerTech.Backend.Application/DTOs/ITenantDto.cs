using MongoDB.Bson;

namespace DomnerTech.Backend.Application.DTOs;

public interface ITenantDto
{
    string CompanyId { get; set; }
}