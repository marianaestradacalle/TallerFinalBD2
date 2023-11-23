using Application.Interfaces.Infrastructure;
using AutoMapper;
using Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace Infrastructure.Services.MongoDB;
public class NotasAdapter : INotasRepository
{
    private readonly Context _mongodb;
    private readonly IMapper _mapper;
    public NotasAdapter(Context context, IMapper mapper)
    {
        _mongodb = context;
        _mapper = mapper;
    }
    public async Task<string> AddNote(Notes notes)
    {
        var newPayment = _mapper.Map<Notes>(notes);

        newPayment.Id = ObjectId.GenerateNewId().ToString();

        var document = new BsonDocument {
           {"_id",  newPayment.Id  },
           { "title",newPayment.Title },
           { "state", newPayment.State },
           { "creationDate", newPayment.CreationDate },
           { "lastUpdateDate", newPayment.LastUpdateDate },
           { "creatorUser", newPayment.CreatorUser },
           { "updaterUser", newPayment.UpdaterUser },
           { "listId", newPayment.ListId },
          };
        await _mongodb.Note.InsertOneAsync(document);
        return newPayment.Id;
    }
    public async Task<dynamic> GetAllNotes()
    {
        return await Task.Run(() =>
        {
            var filter = Builders<BsonDocument>.Filter.Eq("creatorUser", "userTest");
            List<BsonDocument> datos = _mongodb.Note.Find<BsonDocument>(filter).ToList();
            var settings = new JsonWriterSettings { Indent = true };
            var jsonOutput = datos.ToJson(settings);

            return _mapper.Map<dynamic>(jsonOutput);
        });
    }
}
