using System.ComponentModel.DataAnnotations;

namespace DomnerTech.Backend.Application.DTOs;

public record BaseRequest
{
    [Required]
    public Guid UserId { get; set; }
}