using System.Security;
using System.Security.Cryptography;
using System.Text;
using DomnerTech.Backend.Application.Json;
using DomnerTech.Backend.Application.Pagination;

namespace DomnerTech.Backend.Infrastructure.Pagination;

public sealed class HmacCursorSerializer(string secret) : ICursorSerializer
{
    private readonly byte[] _secret = Encoding.UTF8.GetBytes(secret);

    public string Serialize<T>(T payload)
    {
        var json = JsonConvert.SerializeObject(payload);
        var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        using var hmac = new HMACSHA256(_secret);
        var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadBase64));

        return $"{payloadBase64}.{Convert.ToBase64String(signature)}";
    }

    public T Deserialize<T>(string cursor)
    {
        var parts = cursor.Split('.');
        if (parts.Length != 2)
            throw new SecurityException("Invalid cursor");

        var payload = parts[0];
        var signature = parts[1];

        using var hmac = new HMACSHA256(_secret);
        var computed = Convert.ToBase64String(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(signature),
                Encoding.UTF8.GetBytes(computed)))
            throw new SecurityException("Cursor tampered");

        var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        return JsonConvert.DeserializeObject<T>(json)!;
    }
}