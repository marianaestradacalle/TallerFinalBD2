using Application.Interfaces.Infrastructure;
using AutoMapper;
using Core.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Infrastructure.Services.MongoDB;

public class NotasAdapter : INotasRepository
{
    private readonly IContext _mongodb;
    private readonly IMapper _mapper;

    public NotasAdapter(IContext context, IMapper mapper)
    {
        _mongodb = context;
        _mapper = mapper;
    }

  /*  public async Task<string> AddNote(Notes notes)
    {
        var newPayment = _mapper.Map<Notes>(notes);

        newPayment.Id = ObjectId.GenerateNewId().ToString();

        await _mongodb.Note.InsertOneAsync(newPayment);

        return newPayment.Id;
    }*/
    public async Task<dynamic> GetAllNotes()
    {
        IAsyncCursor<Notes> asyncCursor = await _mongodb.Note.FindAsync(x => x.Id == "64d55183a237347e90ca00f2");
        Notes notes  = await asyncCursor.FirstOrDefaultAsync();
        return _mapper.Map<dynamic>(notes);

    }

}
