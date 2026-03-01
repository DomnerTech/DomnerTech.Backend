using DomnerTech.Backend.Application;
using MongoDB.Driver;

namespace DomnerTech.Backend.Infrastructure.MongoDb;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
}

public sealed class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(MongoDbConfig mongoDbConfig)
    {
        var clientSetting = MongoClientSettings.FromConnectionString(mongoDbConfig.ConnectionUri);
        var client = new MongoClient(clientSetting);
        Database = client.GetDatabase(mongoDbConfig.Database);
    }
}