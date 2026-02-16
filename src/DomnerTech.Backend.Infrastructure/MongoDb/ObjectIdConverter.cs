using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DomnerTech.Backend.Infrastructure.MongoDb;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    /// <inheritdoc />
    public override ObjectId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        // Handle null
        if (reader.TokenType == JsonTokenType.Null)
            return ObjectId.Empty;

        // Expect string
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Unexpected token parsing ObjectId. Token: {reader.TokenType}");

        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return ObjectId.Empty;

        return ObjectId.TryParse(value, out var objectId)
            ? objectId
            : ObjectId.Empty;
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        ObjectId value,
        JsonSerializerOptions options)
    {
        if (value == ObjectId.Empty)
        {
            writer.WriteStringValue(string.Empty);
            return;
        }
        writer.WriteStringValue(value.ToString());
    }
}