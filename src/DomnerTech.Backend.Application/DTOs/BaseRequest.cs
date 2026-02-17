namespace DomnerTech.Backend.Application.DTOs;

public record BaseRequest
{
    public required string UserId { get; set; }
}

public record BaseTenantRequest
{
    public required string CompanyId { get; set; }

}