using DomnerTech.Backend.Application;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.MongoDb;

public interface IMongoDbContext : IDisposable
{
    IMongoDatabase Database { get; }
}

public sealed class MongoDbContext : IMongoDbContext
{
    private readonly MongoClient _client;

    public IMongoDatabase Database { get; }

    public MongoDbContext(MongoDbConfig mongoDbConfig)
    {
        var clientSetting = MongoClientSettings.FromConnectionString(mongoDbConfig.ConnectionUri);
        _client = new MongoClient(clientSetting);
        Database = _client.GetDatabase(mongoDbConfig.Database);
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}