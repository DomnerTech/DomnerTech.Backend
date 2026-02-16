using DomnerTech.Backend.Domain;
using MongoDB.Driver;
using System.Reflection;

namespace DomnerTech.Backend.Application.Extensions;

public static class MongoCollectionExtension
{
    public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase database)
    {
        var collectionName = GetCollectionName<T>(); // Simple pluralization
        return database.GetCollection<T>(collectionName);
    }

    public static string GetCollectionName<T>()
    {
        return GetCollectionName(typeof(T));
    }

    public static string GetCollectionName(Type type)
    {
        var attribute = type.GetCustomAttribute<MongoCollectionAttribute>();

        if (attribute == null)
            throw new InvalidOperationException(
                $"MongoCollectionAttribute is not defined on {type.Name}");

        return attribute.CollectionName;
    }
}