using DomnerTech.Backend.Domain.ValueObjects;

namespace DomnerTech.Backend.Application.DTOs;

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string Village { get; set; } = "N/A";
    public string Commune { get; set; } = "N/A";
    public string District { get; set; } = "N/A";
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = "N/A";
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public static class AddressExtensions
{
    public static AddressValueObject ToValueObject(this AddressDto dto)
    {
        return new AddressValueObject
        {
            Street = dto.Street,
            Village = dto.Village,
            Commune = dto.Commune,
            District = dto.District,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Country = dto.Country
        };
    }

    public static AddressDto ToDto(this AddressValueObject valueObject)
    {
        return new AddressDto
        {
            Street = valueObject.Street,
            Village = valueObject.Village,
            Commune = valueObject.Commune,
            District = valueObject.District,
            City = valueObject.City,
            State = valueObject.State,
            PostalCode = valueObject.PostalCode,
            Country = valueObject.Country
        };
    }
}