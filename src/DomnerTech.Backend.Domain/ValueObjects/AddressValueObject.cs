using MongoDB.Bson.Serialization.Attributes;

namespace DomnerTech.Backend.Domain.ValueObjects;

[BsonIgnoreExtraElements]
public sealed class AddressValueObject
{
    public required string Street { get; set; }
    public string Village { get; set; } = "N/A";
    public string Commune { get; set; } = "N/A";
    public string District { get; set; } = "N/A";
    public required string City { get; set; }
    public string State { get; set; } = "N/A";
    public required string PostalCode { get; set; }
    public required string Country { get; set; }

    public AddressValueObject()
    {
    }

    public AddressValueObject(string street, string city, string postalCode, string country)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public override bool Equals(object? obj)
    {
        return obj is AddressValueObject address &&
               Street == address.Street &&
               Village == address.Village &&
               Commune == address.Commune &&
               District == address.District &&
               City == address.City &&
               State == address.State &&
               PostalCode == address.PostalCode &&
               Country == address.Country;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, Village, Commune, District, City, State, PostalCode, Country);
    }

    public override string ToString()
    {
        return $"{Street}, {Village}, {Commune}, {District}, {City}, {State}, {PostalCode}, {Country}";
    }

    /// <summary>
    /// NA, Thmey, Phsar, Phsar, Boribo, Kammpong Chhnang, , 1234, Cambodia
    /// </summary>
    /// <param name="addressString"></param>
    /// <returns></returns>
    public static AddressValueObject? Parse(string? addressString)
    {
        if (string.IsNullOrWhiteSpace(addressString))
            return null;

        var span = addressString.AsSpan();

        var length = span.Length;
        var part = 0;
        var start = 0;

        // Field boundaries
        int s0 = 0, e0 = 0;
        int s1 = 0, e1 = 0;
        int s2 = 0, e2 = 0;
        int s3 = 0, e3 = 0;
        int s4 = 0, e4 = 0;
        int s5 = 0, e5 = 0;
        int s6 = 0, e6 = 0;

        for (var i = 0; i < length; i++)
        {
            if (span[i] != ',') continue;
            switch (part)
            {
                case 0: s0 = start; e0 = i; break;
                case 1: s1 = start; e1 = i; break;
                case 2: s2 = start; e2 = i; break;
                case 3: s3 = start; e3 = i; break;
                case 4: s4 = start; e4 = i; break;
                case 5: s5 = start; e5 = i; break;
                case 6: s6 = start; e6 = i; break;
                default: return null; // too many parts
            }

            part++;
            start = i + 1;
        }

        // Last part
        if (part != 7)
            return null;

        var s7 = start;

        return new AddressValueObject
        {
            Street = SliceTrim(span, s0, e0),
            Village = SliceTrim(span, s1, e1),
            Commune = SliceTrim(span, s2, e2),
            District = SliceTrim(span, s3, e3),
            City = SliceTrim(span, s4, e4),
            State = SliceTrim(span, s5, e5),
            PostalCode = SliceTrim(span, s6, e6),
            Country = SliceTrim(span, s7, length)
        };

        static string SliceTrim(ReadOnlySpan<char> s, int start, int end)
        {
            var slice = s[start..end].Trim();
            return slice.ToString(); // only allocation point
        }
    }

    /// <summary>
    /// Tries to parse a string to AddressValueObject.
    /// </summary>
    public static bool TryParse(string? addressString, out AddressValueObject? result)
    {
        result = null;
        try
        {
            result = Parse(addressString);
            return result != null;
        }
        catch
        {
            return false;
        }
    }
}