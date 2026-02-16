using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DomnerTech.Backend.Infrastructure.MongoDb;

public static class Extensions
{
    private static readonly Lock Lock = new();
    private static bool _conventionsRegistered;

    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDbContextFactory, MongoDbContextFactory>();
        RegisterConventions();
        return services;
    }

    private static void RegisterConventions()
    {
        lock (Lock)
        {
            if (_conventionsRegistered) return;
            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            ConventionRegistry.Register("common_conventions", new MongoDbConventions(), _ => true);
            _conventionsRegistered = true;
        }
    }

    private sealed class MongoDbConventions : IConventionPack
    {
        public IEnumerable<IConvention> Conventions =>
        [
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
            new CamelCaseElementNameConvention()
        ];
    }
}
