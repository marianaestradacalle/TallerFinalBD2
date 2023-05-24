using Application.DTOs.NotesLists;
using Application.Interfaces.Services;
using AutoMapper;
using Grpc.Core;
using GrpcNoteListsService;

namespace GrpcApiService.ServiceName.Services;
public class NoteListsService : GrpcNoteListsService.NoteListsService.NoteListsServiceBase
{
    private readonly ILogger<NoteListsService> _logger;
    private readonly INoteListsUseCase _listsUseCases;
    private readonly IMapper _mapper;


    public NoteListsService(ILogger<NoteListsService> logger,
        INoteListsUseCase listsUseCases,
    IMapper mapper)
    {
        _logger = logger;
        _listsUseCases = listsUseCases;
        _mapper = mapper;
    }
    public override async Task<NoteList> Save(AddNoteList request,
        ServerCallContext context)
    {
        var response = await _listsUseCases.CreateNoteList(_mapper.Map<NoteListInput>(request));

        return _mapper.Map<NoteList>(response);
    }

    public override async Task<SimplifiedNoteLists> GetAll(Empty request,
    ServerCallContext context)
    {
        var response = await _listsUseCases.GetNoteLists();

        return _mapper.Map<SimplifiedNoteLists>(response);
    }

    public override async Task<NoteList> GetOne(RequestId request,
    ServerCallContext context)
    {
        var response = await _listsUseCases.GetNoteList(_mapper.Map<string>(request));

        return _mapper.Map<NoteList>(response);
    }

    public override async Task<Empty> Delete(RequestId request,
    ServerCallContext context)
    {
        _= _listsUseCases.DeleteNoteList(_mapper.Map<string>(request));
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> DeleteChecked(Empty request,
        ServerCallContext context)
    {
        _= _listsUseCases.DeleteCheckedNoteLists();
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> AddNote(RequestIds request,
    ServerCallContext context)
    {
        
       _ = _listsUseCases.AddNoteToList(_mapper.Map<string>(request.NoteId), _mapper.Map<RelatedNoteInput>(request.ListId));
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> RemoveNote(RequestIds request, 
    ServerCallContext context)
    {
        _= _listsUseCases.RemoveNoteFromList(_mapper.Map<string>(request.NoteId), _mapper.Map<string>(request.ListId));
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> Check(Empty request,
    ServerCallContext context)
    {
        _= _listsUseCases.CheckedNoteLists();
        Empty response = new Empty();
        return response;
    }

    public override async Task<NoteList> Update(ModifiedNoteList request, ServerCallContext context)
    {
        ModifiedNoteListsInput modifiedNoteListsInput = new ModifiedNoteListsInput();

        modifiedNoteListsInput.Name = request.Name;
        modifiedNoteListsInput.State = request.State;


        var response = _listsUseCases.UpdateNoteLists(_mapper.Map<string>(request.Id), modifiedNoteListsInput);
        return _mapper.Map<NoteList>(response);

    }

}
