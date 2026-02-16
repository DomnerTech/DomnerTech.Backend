namespace DomnerTech.Backend.Application.DTOs;

public interface IBaseDto
{
    string Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}