using Application.DTOs.Notes;
using Application.DTOs.NotesLists;
using AutoMapper;
using Core.Enumerations;

namespace Application.DTOs;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NoteInput, Core.Entities.Notes>()
             .ConvertUsing(x => new Core.Entities.Notes(Guid.NewGuid().ToString(), x.Title == null ? "GenericNote" : x.Title, NoteStates.NOTCHECKED, DateTime.UtcNow, null, "Test", "", ""));

        CreateMap<Core.Entities.Notes, NoteOutput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

        CreateMap<Core.Entities.Notes, SimplifiedNoteOutput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

        CreateMap<NoteListInput, Core.Entities.NoteLists>()
             .ConvertUsing(x => new Core.Entities.NoteLists(Guid.NewGuid().ToString(), x.Name == "" ? "GenericList" : x.Name, NoteStates.NOTCHECKED, DateTime.UtcNow, null, "Test", ""));

        CreateMap<Core.Entities.NoteLists, NoteListOutput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

        CreateMap<Core.Entities.NoteLists, SimplifiedNoteListOutput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
    }
}