using AutoMapper;
using Core.Entities;

namespace Application.DTOs;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventoDTO, EventoEntity>().ReverseMap();
        CreateMap<FacultadDTO, FacultadEntity>().ReverseMap();
        CreateMap<AsistenteDTO, AsistenteEntity>().ReverseMap();
        CreateMap<ComentarioDTO, ComentarioEntity>().ReverseMap();

        CreateMap<AreaDTO, AreaEntity>();
        CreateMap<SedeDTO, SedeEntity>();
        CreateMap<CiudadDTO, CiudadEntity>();
    }
}