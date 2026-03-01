using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Json;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (ObjectId.TryParse(s, out var oid)) return oid;
            throw new JsonException($"Invalid ObjectId: {s}");
        }
        if (reader.TokenType == JsonTokenType.Null) return ObjectId.Empty;
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}