using Core.Entities.Mongo;
using MongoDB.Driver;

namespace Application.Interfaces.Infrastructure;
public interface IContext
{
    public IMongoCollection<Notes> Note { get; }
}