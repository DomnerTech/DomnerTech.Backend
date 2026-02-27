namespace DomnerTech.Backend.Application.Services;

public interface ITenantService : IBaseService
{
    string CompanyId { get; }
    void SetTenant(string companyId);
}