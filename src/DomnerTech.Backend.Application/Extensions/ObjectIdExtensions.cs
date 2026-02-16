using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Extensions;

public static class ObjectIdExtensions
{
    /// <param name="id">The string representation of the ObjectId to convert. Leading and trailing whitespace are ignored. Can be null.</param>
    extension(string? id)
    {
        /// <summary>
        /// Converts the specified string to an ObjectId instance.
        /// </summary>
        /// <remarks>If the input string is not a valid ObjectId format, the returned ObjectId will not represent
        /// a valid identifier. To verify validity, use ObjectId.TryParse before calling this method.</remarks>
        /// <returns>An ObjectId parsed from the specified string. If the string is null, empty, or not a valid ObjectId, returns an
        /// ObjectId with all zero bytes.</returns>
        public ObjectId ToObjectId()
        {
            _ = ObjectId.TryParse(id?.Trim() ?? string.Empty, out var objectId);
            return objectId;
        }

        /// <summary>
        /// Converts the specified string to a nullable ObjectId instance.
        /// </summary>
        /// <returns>A nullable ObjectId parsed from the string. Returns null if the input is null, empty, or not a valid ObjectId
        /// format.</returns>
        public ObjectId? ToObjectIdNullable()
        {
            if (id == null)
            {
                return null;
            }

            _ = ObjectId.TryParse(id.Trim(), out var objectId);
            return objectId;
        }
    }
}