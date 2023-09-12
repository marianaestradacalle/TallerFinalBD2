using Application.Interfaces.Infrastructure;
using Core.Entities.Mongo;
using MongoDB.Driver;

namespace Infrastructure.Services.MongoDB;
public class Context : IContext
{
    private static volatile Context _instance;
    private static readonly object SyncLock = new object();
    private readonly IMongoDatabase _database;

    public Context(string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);

        _database = mongoClient.GetDatabase(databaseName);
    }
    public static Context GetMongoDatabase(string connectionString, string databaseName)
    {
        if (_instance is null)
            lock (SyncLock)
            {
                if (_instance == null)
                    _instance = new Context(connectionString, databaseName);
            }

        return _instance;
    }
    public IMongoCollection<Notes> Note => _database.GetCollection<Notes>("Notes");
}