using Application.DTOs;

namespace Application.Interfaces.Infrastructure;
public interface IEventoRepository
{
    Task<bool> CrearEvento(EventoDTO evento);
    Task<bool> ActualizarEvento(EventoDTO evento);
    Task<bool> EliminarEvento(string idEvento);
    List<EventoDTO> ObtenerEventos();
    Task<EventoDTO> ObtenerEventoPorId(string IdEvento);
}
