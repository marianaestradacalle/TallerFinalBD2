using Application.DTOs;
using Application.Interfaces.Infrastructure;
using AutoMapper;

namespace Infrastructure.Services.MSQLServer;
public class AsistenteAdapter : IAsistenteRepository
{
    private readonly IMapper _mapper;
    public AsistenteAdapter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<AsistenteDTO> ObtenerAsistentePorIdentificacion(string id)
    {
        throw new NotImplementedException();
    }

    public Task ObtenerAsistentes()
    {
        throw new NotImplementedException();
    }
}
