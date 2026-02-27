using DomnerTech.Backend.Application.Services;

namespace DomnerTech.Backend.Infrastructure.Services;

public class TenantService : ITenantService
{
    public string CompanyId { get; private set; } = string.Empty;
    public void SetTenant(string companyId)
    {
        CompanyId = companyId;
    }
}