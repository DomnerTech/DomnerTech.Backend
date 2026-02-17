namespace DomnerTech.Backend.Application;

public sealed class AppSettings
{
    public BearerAuthConfig BearerAuth { get; set; } = new();
    public SwaggerConfig Swagger { get; set; } = new();
    public RedisConfig Redis { get; set; } = new();
    public ConnectionStringConfig ConnectionStrings { get; set; } = new();
    public List<HttpClientOptionsItem> ClientApis { get; set; } = [];

    public MongoDatabases MongoDatabases { get; set; } = new();
}

public class BearerAuthConfig
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiryInMinutes { get; set; }
}

public class RedisConfig
{
    public bool Enabled { get; set; }
    public List<string> EndPoints { get; set; } = [];
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int ConnectTimeout { get; set; }
    public RedisKeyConfig RedisKeySetting { get; set; } = new();
    public bool Ssl { get; set; }
    public string InstanceName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
}

public class RedisKeyConfig;

public class ConnectionStringConfig
{
    public string CacheDb { get; set; } = string.Empty;
}

public class HttpClientOptionsItem
{
    public string Key { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public Dictionary<string, string> DefaultHeaders { get; set; } = [];
}

public class SwaggerConfig
{
    public bool Enable { get; set; }
    public HashSet<string> Urls { get; set; } = [];
}

public class MongoDatabases
{
    public Dictionary<string, MongoDbConfig> ConnectionStrings { get; set; } = [];
}

public class MongoDbConfig
{
    public string ConnectionUri { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}