using Application.DTOs;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;

namespace Application.Services;
public class EventoUseCase : IEventoUseCase
{
    private readonly IEventoRepository _eventoRepository;

    public EventoUseCase(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    public async Task<bool> ActualizarEvento(EventoDTO evento)
    {
        try
        {
            return await _eventoRepository.ActualizarEvento(evento);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> CrearEvento(EventoDTO evento)
    {
        try
        {
            return await _eventoRepository.CrearEvento(evento);
        }
        catch (Exception)
        {
            throw;
        };
    }

    public async Task<bool> EliminarEvento(string idEvento)
    {
        try
        {
            return await _eventoRepository.EliminarEvento(idEvento);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EventoDTO> ObtenerEventoPorId(string IdEvento)
    {
        try
        {
            return await _eventoRepository.ObtenerEventoPorId(IdEvento);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<EventoDTO> ObtenerEventos()
    {
        try
        {
            return _eventoRepository.ObtenerEventos();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
