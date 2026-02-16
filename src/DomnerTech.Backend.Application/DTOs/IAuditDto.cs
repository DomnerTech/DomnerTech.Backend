namespace DomnerTech.Backend.Application.DTOs;

public interface IAuditDto
{
    string? UpdatedBy { get; set; }
    string? DeletedBy { get; set; }
}