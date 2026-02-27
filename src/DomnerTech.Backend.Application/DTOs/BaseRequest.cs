namespace DomnerTech.Backend.Application.DTOs;

public interface IBaseRequest
{
    string UserReqId { get; set; }
}

public interface IBaseTenantRequest
{
    string CompanyId { get; set; }
}