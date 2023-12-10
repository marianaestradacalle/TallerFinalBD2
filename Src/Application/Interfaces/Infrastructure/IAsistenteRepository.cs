using Application.DTOs;

namespace Application.Interfaces.Infrastructure;
public interface IAsistenteRepository
{
    Task ObtenerAsistentes();
    Task<AsistenteDTO> ObtenerAsistentePorIdentificacion(string id);

}
