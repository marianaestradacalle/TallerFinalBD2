using Core.Entities;
using MongoDB.Driver;

namespace Infrastructure.Services.MongoDB;
public class Context
{
    private static volatile Context? _instance;
    private static readonly object SyncLock = new();
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
                _instance ??= new Context(connectionString, databaseName);
            }

        return _instance;
    }
    public IMongoCollection<EventoEntity> Evento => _database.GetCollection<EventoEntity>("Eventos");
}