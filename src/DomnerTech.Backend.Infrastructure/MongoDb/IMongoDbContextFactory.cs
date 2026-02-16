using System.Collections.Concurrent;
using DomnerTech.Backend.Application;

namespace DomnerTech.Backend.Infrastructure.MongoDb;

public interface IMongoDbContextFactory
{
    IMongoDbContext Create(string name);
}

public sealed class MongoDbContextFactory(AppSettings appSettings) : IMongoDbContextFactory
{
    private readonly MongoDatabases _mongoDatabases = appSettings.MongoDatabases;
    private readonly ConcurrentDictionary<string, Lazy<IMongoDbContext>> _contexts = new();

    public IMongoDbContext Create(string dbName)
    {
        var lazyContext = _contexts.GetOrAdd(dbName, _ => new Lazy<IMongoDbContext>(() => CreateMongoDbContext(dbName)));
        return lazyContext.Value;
    }

    private MongoDbContext CreateMongoDbContext(string dbName)
    {
        var dbConfig = GetMongoDbConfig(dbName);
        return new MongoDbContext(dbConfig);
    }

    private MongoDbConfig GetMongoDbConfig(string dbName)
    {
        return !_mongoDatabases.ConnectionStrings.TryGetValue(dbName, out var dbConfig) ? throw new ArgumentException($"MongoDB configuration for '{dbName}' not found.") : dbConfig;
    }
}