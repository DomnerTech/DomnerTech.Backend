using System.Security;
using System.Security.Cryptography;
using System.Text;
using DomnerTech.Backend.Application.Json;
using DomnerTech.Backend.Application.Pagination.KeySetPaging;

namespace DomnerTech.Backend.Infrastructure.Pagination.KeysetPaging;

public sealed class HmacCursorSerializer(string secret) : ICursorSerializer
{
    private readonly byte[] _secret = Encoding.UTF8.GetBytes(secret);

    /// <summary>
    /// Converts byte array to URL-safe Base64 string (Base64Url encoding).
    /// Replaces '+' with '-', '/' with '_', and removes padding '='.
    /// </summary>
    private static string ToBase64Url(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    /// <summary>
    /// Converts URL-safe Base64 string back to byte array.
    /// </summary>
    private static byte[] FromBase64Url(string base64Url)
    {
        var base64 = base64Url
            .Replace('-', '+')
            .Replace('_', '/');

        // Add padding if needed
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    public string Serialize<T>(T payload)
    {
        var json = JsonConvert.SerializeObject(payload);
        var payloadBase64 = ToBase64Url(Encoding.UTF8.GetBytes(json));

        using var hmac = new HMACSHA256(_secret);
        var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadBase64));

        return $"{payloadBase64}.{ToBase64Url(signature)}";
    }

    public T Deserialize<T>(string cursor)
    {
        var parts = cursor.Split('.');
        if (parts.Length != 2)
            throw new SecurityException("Invalid cursor");

        var payload = parts[0];
        var signature = parts[1];

        using var hmac = new HMACSHA256(_secret);
        var computed = ToBase64Url(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(signature),
                Encoding.UTF8.GetBytes(computed)))
            throw new SecurityException("Cursor tampered");

        var json = Encoding.UTF8.GetString(FromBase64Url(payload));
        return JsonConvert.DeserializeObject<T>(json)!;
    }
}