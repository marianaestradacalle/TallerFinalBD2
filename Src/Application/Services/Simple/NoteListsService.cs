using Application.Common.Utilities;
using Application.DTOs.NotesLists;
using Application.Interfaces.Infraestructure;
using Application.Interfaces.Services;
using AutoMapper;
using Common.Helpers.Exceptions;
using Core.Entities;
using Core.Enumerations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Simple;
public class NoteListsService : INoteListsUseCase
{
    private readonly IGenericRepositoryAdapter<NoteLists> _noteListRepository;
    private readonly ILogger<NoteListsService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepositoryAdapter<Notes> _notesRepository;
    private readonly IUnitWork _unitWork;
    private readonly BusinessSettings _settings;

    public NoteListsService(IGenericRepositoryAdapter<NoteLists> noteListRepository,
        ILogger<NoteListsService> logger,
        IMapper mapper,
        INotesUseCase notesUseCases,
        IGenericRepositoryAdapter<Notes> notesRepository,
        IUnitWork unitWork,
        IOptionsMonitor<BusinessSettings> settings)
    {
        _noteListRepository = noteListRepository;
        _logger = logger;
        _mapper = mapper;
        _notesRepository = notesRepository;
        _unitWork = unitWork;
        _settings = settings.CurrentValue;
    }

    public async Task<NoteListOutput> CreateNoteList(NoteListInput data)
    {
        NoteLists noteLists = _mapper.Map<NoteListInput, NoteLists>(data);
        await _noteListRepository.AddAsync(noteLists);
        await _unitWork.SaveAsync();
        return _mapper.Map<NoteLists, NoteListOutput>(noteLists);
    }
    public async Task<IEnumerable<SimplifiedNoteListOutput>> GetNoteLists()
    {
        IEnumerable<NoteLists> simplifiedNoteListResponse = await _noteListRepository.GetAllAsync()
                                ?? throw BusinessException.Throw(_settings, _settings.ServiceExceptions.Where(x => x.Id == 
                                BusinessExceptionTypes.NotControlledException.ToString()).Select(x => x.Code).First());

        return _mapper.Map<IEnumerable<NoteLists>, IEnumerable<SimplifiedNoteListOutput>>(simplifiedNoteListResponse);
    }
    public async Task<NoteListOutput> GetNoteList(string listId)
    {
        await CheckIfNoteListExists(listId);
        var noteListResponse = await _noteListRepository.FindByIdAsync(listId)
                            ?? throw BusinessException.Throw(_settings, _settings.ServiceExceptions.Where(x => x.Id == 
                            BusinessExceptionTypes.NotControlledException.ToString()).Select(x => x.Code).First());
        return _mapper.Map<NoteLists, NoteListOutput>(noteListResponse);
    }
    public async Task<bool> DeleteNoteList(string listId)
    {
        await CheckIfNoteListExists(listId);
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync();
        if (notesResponse.Where(x => x.State != NoteStates.CHECKED).Count() == 0)
        {
            foreach (var item in notesResponse.Where(x => x.State == NoteStates.CHECKED))
            {
                item.ListId = "";
                _notesRepository.Update(item);
                await _unitWork.SaveAsync();
            }
        }

        _noteListRepository.Delete(listId);
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> DeleteCheckedNoteLists()
    {
        IEnumerable<NoteLists> noteListsResponse = (IEnumerable<NoteLists>)await _noteListRepository.GetAllAsync(x => x.State == NoteStates.CHECKED);
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync(x => x.State == NoteStates.CHECKED);
        noteListsResponse = (from x in noteListsResponse
                             where (from y in notesResponse
                                    select y.ListId).Contains(x.Id)
                             select x).ToList();
        notesResponse = (from x in notesResponse
                         where (from y in noteListsResponse
                                select y.Id).Contains(x.ListId)
                         select x).ToList();
        foreach (var item in noteListsResponse)
        {
            _noteListRepository.Delete(item.Id);
        }
        foreach (var item in notesResponse)
        {
            item.ListId = "";
            _notesRepository.Update(item);
        }
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> AddNoteToList(string listId, RelatedNoteInput data)
    {
        await CheckIfNoteListExists(listId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(data.NoteId);
        notesResponse.ListId = !String.IsNullOrEmpty(notesResponse.ListId) ? notesResponse.ListId : listId;
        _notesRepository.Update(notesResponse);
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> RemoveNoteFromList(string listId, string noteId)
    {
        await CheckIfNoteListExists(listId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(noteId);
        if (notesResponse.ListId == listId.Trim())
        {
            notesResponse.ListId = "";
            _notesRepository.Update(notesResponse);
        }
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> CheckedNoteLists()
    {
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync();
        foreach (var item in notesResponse.Where(x => x.State == NoteStates.NOTCHECKED))
        {
            item.State = NoteStates.CHECKED;
            _notesRepository.Update(item);
            await _unitWork.SaveAsync();
        }
        IEnumerable<NoteLists> notesListResponse = (IEnumerable<NoteLists>)await _noteListRepository.GetAllAsync();
        foreach (var item in notesListResponse.Where(x => x.State == NoteStates.NOTCHECKED))
        {
            item.State = NoteStates.CHECKED;
            _noteListRepository.Update(item);
            await _unitWork.SaveAsync();
        }
        return true;
    }

    public async Task<bool> UpdateNoteLists(string listId, ModifiedNoteListsInput data)
    {
        await CheckIfNoteListExists(listId);
        NoteLists notesListResponse = await _noteListRepository.FindByIdAsync(listId);
        notesListResponse.Name = string.IsNullOrEmpty(data.Name) ? notesListResponse.Name : data.Name;
        notesListResponse.State = data.State <= 0 ? notesListResponse.State : (NoteStates)data.State;
        notesListResponse.LastUpdateDate = DateTime.UtcNow;
        notesListResponse.UpdaterUser = "Test";
        _noteListRepository.Update(notesListResponse);
        await _unitWork.SaveAsync();
        return true;
    }
    #region private
    public async Task CheckIfNoteListExists(string listId)
    {
        NoteLists result = await _noteListRepository.FindByIdAsync(listId)
            ?? throw BusinessException.Throw(_settings, _settings.ServiceExceptions.Where(x => x.Id == BusinessExceptionTypes.NotControlledException.ToString()).Select(x => x.Code).First());
    }
    #endregion private
}
