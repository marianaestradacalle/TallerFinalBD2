using Application.DTOs.Notes;
using Application.Interfaces.Services;
using AutoMapper;
using Grpc.Core;
using GrpcNotesService;

namespace GrpcApiService.ServiceName.Services;
public class NotesService : GrpcNotesService.NotesService.NotesServiceBase
{
    private readonly ILogger<NotesService> _logger;
    private readonly INotesService _notesUseCases;
    private readonly IMapper _mapper;


    public NotesService(ILogger<NotesService> logger,
        INotesService notesUseCases,
        IMapper mapper)
    {
        _logger = logger;
        _notesUseCases = notesUseCases;
        _mapper = mapper;
    }
    public override async Task<Note> Save(AddNote request,
         ServerCallContext context)
    {
        var response = await _notesUseCases.CreateNote(_mapper.Map<NoteInput>(request));

        return _mapper.Map<Note>(response);
    }

    public override async Task<SimplifiedNotes> GetAll(Empty request,
    ServerCallContext context)
    {
        var response = await _notesUseCases.GetNotes();

        return _mapper.Map<SimplifiedNotes>(response);
    }

    public override async Task<Note> GetOne(RequestId request,
    ServerCallContext context)
    {
        var response = await _notesUseCases.GetNote(_mapper.Map<string>(request));

        return _mapper.Map<Note>(response);
    }

    public override async Task<Empty> Delete(RequestId request,
    ServerCallContext context)
    {
        _= _notesUseCases.DeleteNote(_mapper.Map<string>(request));
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> DeleteChecked(Empty request,ServerCallContext context)
    {
      _=_notesUseCases.DeleteCheckedIsolateNotes();
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> ChangeState(NoteState request,ServerCallContext context)
    {
        _= _notesUseCases.ChangeNoteState(_mapper.Map<string>(request.Id), _mapper.Map<NoteStateInput>(request.State));
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> Update(ModifiedNote request,ServerCallContext context)
    {
        ModifiedNoteInput modifiedNoteInput = new ModifiedNoteInput();
        
            modifiedNoteInput.Title = request.Title;
            modifiedNoteInput.State = request.State;
        
       
        _ = _notesUseCases.UpdateNote(_mapper.Map<string>(request.Id), modifiedNoteInput);
        Empty response = new Empty();
        return response;

    }

}
