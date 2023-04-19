using AutoMapper;
using Application.DTOs.Notes;
using GrpcNotesService;

namespace GrpcApiService.ServiceName;
public class GRPCProfile : Profile
{
    public GRPCProfile()
    {

        CreateMap<AddNote, NoteInput>();
        CreateMap<NoteOutput, Note>();
        CreateMap<NoteState, NoteStateInput>();
        CreateMap<ModifiedNote, ModifiedNoteInput>();
        CreateMap<SimplifiedNoteOutput, SimplifiedNotes>();

        CreateMap<AddNote, NoteInput>();
        CreateMap<NoteOutput, Note>();
        CreateMap<NoteState, NoteStateInput>();
        CreateMap<ModifiedNote, ModifiedNoteInput>();
    }
}
