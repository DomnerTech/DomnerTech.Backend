using System.Text.Json;

namespace DomnerTech.Backend.Application.Json;

public static class JsonConvert
{
    public static string SerializeObject<T>(T obj, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(obj , options ?? DefaultJsonSerializerSettings.SnakeCase);
    }

    public static byte[] SerializeToUtf8Bytes<T>(T obj, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.SerializeToUtf8Bytes(obj , options ?? DefaultJsonSerializerSettings.SnakeCase);
    }

    public static T? DeserializeObject<T>(string json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(json, options ?? DefaultJsonSerializerSettings.SnakeCase);
    }

    public static object? DeserializeObject(string json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<object>(json, options ?? DefaultJsonSerializerSettings.SnakeCase);
    }

    public static object? DeserializeObject(string json, Type type, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize(json, type, options ?? DefaultJsonSerializerSettings.SnakeCase);
    }

    public static async Task<T?> DeserializeAsync<T>(Stream utf8Json, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<T>(utf8Json, options ?? DefaultJsonSerializerSettings.SnakeCase, cancellationToken);
    }
}