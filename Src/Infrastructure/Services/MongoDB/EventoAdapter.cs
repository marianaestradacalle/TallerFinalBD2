using Application.DTOs;
using Application.Interfaces.Infrastructure;
using AutoMapper;
using Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Services.MongoDB;
public class EventoAdapter : IEventoRepository
{
    private readonly Context _mongodb;
    private readonly IMapper _mapper;
    public EventoAdapter(Context context, IMapper mapper)
    {
        _mongodb = context;
        _mapper = mapper;
    }

    public async Task<bool> ActualizarEvento(EventoDTO evento)
    {
        EventoEntity eventoEntity = _mapper.Map<EventoEntity>(evento);
        FilterDefinition<EventoEntity> filter = Builders<EventoEntity>.Filter.Eq("idEvento", new ObjectId(evento.IdEvento));

        UpdateDefinition<EventoEntity> valuesForUpdate = Builders<EventoEntity>.Update.Set("Eventos", evento);

        _ = await _mongodb.Evento.FindOneAndUpdateAsync(filter, valuesForUpdate,
             new FindOneAndUpdateOptions<EventoEntity>
             {
                 ReturnDocument = ReturnDocument.After
             });

        return true;
    }

    public async Task<bool> CrearEvento(EventoDTO evento)
    {
        EventoEntity eventoEntity = _mapper.Map<EventoEntity>(evento);

        await _mongodb.Evento.InsertOneAsync(eventoEntity);
        return true;
    }

    public async Task<bool> EliminarEvento(string idEvento)
    {
        FilterDefinition<EventoEntity> filter = Builders<EventoEntity>.Filter.Eq("idEvento", new ObjectId(idEvento));

        await _mongodb.Evento.DeleteOneAsync(filter);
        return true;
    }

    public async Task<EventoDTO> ObtenerEventoPorId(string IdEvento)
    {
        FilterDefinition<EventoEntity> filter = Builders<EventoEntity>.Filter.Eq("idEvento", new ObjectId(IdEvento));
        IAsyncCursor<EventoEntity> result = await _mongodb.Evento.FindAsync(filter);
        EventoEntity evento = result.FirstOrDefault();

        return _mapper.Map<EventoDTO>(evento);
    }

    public List<EventoDTO> ObtenerEventos()
    {
        List<EventoEntity> resp = _mongodb.Evento.Find(Builders<EventoEntity>.Filter.Empty).ToList();

        return _mapper.Map<List<EventoDTO>>(resp);
    }
}
