using Application.Common.Utilities;
using Application.DTOs.Notes;
using Application.Interfaces.Infraestructure;
using Application.Interfaces.Services;
using AutoMapper;
using Common.Helpers.Exceptions;
using Core.Entities;
using Core.Enumerations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Simple;
public class NotesService : INotesService
{
    private readonly IGenericRepositoryService<Notes> _notesRepository;
    private readonly ILogger<NotesService> _logger;
    private readonly IMapper _mapper;
    private readonly INotificationServiceBusService _NotificationEventAdapter;
    private readonly BusinessSettings _settings;
    private readonly IUnitWork _unitWork;

    public NotesService(IGenericRepositoryService<Notes> repository,
        ILogger<NotesService> logger,
        IMapper mapper,
        INotificationServiceBusService notificationEventAdapter,
        IOptionsMonitor<BusinessSettings> settings,
        IUnitWork unitWork)
    {
        _notesRepository = repository;
        _logger = logger;
        _mapper = mapper;
        _NotificationEventAdapter = notificationEventAdapter;
        _settings = settings.CurrentValue;
        _unitWork = unitWork;
        _logger.LogInformation("Use case {class} used at: {@time}", nameof(NotesService), DateTimeOffset.Now);
    }
    public async Task<NoteOutput> CreateNote(NoteInput data)
    {
        Notes notes = _mapper.Map<NoteInput, Notes>(data);
        await _notesRepository.AddAsync(notes);
        await _unitWork.SaveAsync();
        return _mapper.Map<Notes, NoteOutput>(notes);
    }
    public async Task DeleteNote(string noteId)
    {
        await checkIfNoteExist(noteId);
        _notesRepository.Delete(noteId);
        await _unitWork.SaveAsync();
    }
    public async Task<IEnumerable<SimplifiedNoteOutput>> GetNotes()
    {
        IEnumerable<Notes> simplifiedNoteResponse = await _notesRepository.GetAllAsync()
                            ?? throw BusinessException.Throw(BusinessExceptionTypes.RecordNotFound, _settings);
        return _mapper.Map<IEnumerable<Notes>, IEnumerable<SimplifiedNoteOutput>>(simplifiedNoteResponse);
    }
    public async Task<NoteOutput> GetNote(string noteId)
    {
        await checkIfNoteExist(noteId);
        var noteResponse = await _notesRepository.FindByIdAsync(noteId)
                        ?? throw BusinessException.Throw(BusinessExceptionTypes.RecordNotFound, _settings);
        return _mapper.Map<Notes, NoteOutput>(noteResponse);
    }
    public async Task DeleteCheckedIsolateNotes()
    {
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync(x => x.State == NoteStates.CHECKED
            && x.ListId.Trim() == "");
        foreach (var item in notesResponse)
        {
            _notesRepository.Delete(item.Id);
        }
        await _unitWork.SaveAsync();
    }
    public async Task ChangeNoteState(string noteId, NoteStateInput data)
    {
        await checkIfNoteExist(noteId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(noteId);
        notesResponse.State = data.State;
        _notesRepository.Update(notesResponse);
        await _unitWork.SaveAsync();
    }
    public async Task UpdateNote(string noteId, ModifiedNoteInput data)
    {
        await checkIfNoteExist(noteId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(noteId);
        notesResponse.Title = string.IsNullOrEmpty(data.Title) ? notesResponse.Title : data.Title;
        notesResponse.State = data.State == null ? notesResponse.State : (NoteStates)data.State;
        notesResponse.LastUpdateDate = DateTime.UtcNow;
        notesResponse.UpdaterUser = "Test";
        _notesRepository.Update(notesResponse);
        await _unitWork.SaveAsync();
    }
    public void Notification()
    {
        _ = _NotificationEventAdapter.GetNotificationSms();
    }
    #region private
    private async Task checkIfNoteExist(string id)
    {
        Notes result = await _notesRepository.FindByIdAsync(id)
            ?? throw BusinessException.Throw(BusinessExceptionTypes.RecordNotFound, _settings);
    }
    #endregion private
}
