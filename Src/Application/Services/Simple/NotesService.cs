using Application.Common.Utilities;
using Application.DTOs.Notes;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;
using AutoMapper;
using Common.Helpers.Exceptions;
using Core.Entities;
using Core.Enumerations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Simple;
public class NotesService : INotesUseCase
{
    private readonly IGenericRepositoryAdapter<Notes> _notesRepository;
    private readonly ILogger<NotesService> _logger;
    private readonly IMapper _mapper;
    private readonly INotificationServiceEventAdapter _NotificationEventAdapter;
    private readonly BusinessSettings _settings;
    private readonly IUnitWork _unitWork;
    private readonly INotasRepository _notasRepository;

    public NotesService(IGenericRepositoryAdapter<Notes> repository,
        ILogger<NotesService> logger,
        IMapper mapper,
        INotificationServiceEventAdapter notificationEventAdapter,
        IOptionsMonitor<BusinessSettings> settings,
        IUnitWork unitWork,
        INotasRepository notasRepository)
    {
        _notesRepository = repository;
        _logger = logger;
        _mapper = mapper;
        _NotificationEventAdapter = notificationEventAdapter;
        _settings = settings.CurrentValue;
        _unitWork = unitWork;
        _notasRepository = notasRepository;
        _logger.LogInformation("Use case {class} used at: {@time}", nameof(NotesService), DateTimeOffset.Now);
        
    }
    public async Task<NoteOutput> CreateNote(NoteInput data)
    {
        Notes notes = _mapper.Map<NoteInput, Notes>(data);
        await _notesRepository.AddAsync(notes);
        await _unitWork.SaveAsync();
        return _mapper.Map<Notes, NoteOutput>(notes);
    }
    public async Task<bool> DeleteNote(string noteId)
    {
        await checkIfNoteExist(noteId);
        _notesRepository.Delete(noteId);
        return true;

    }
    public async Task<IEnumerable<SimplifiedNoteOutput>> GetNotes()
    {
        //prueba parametro mongo provider
        string param = _settings.CRON_SONDA_JOB;
        //prueba consulata mongo driver
        var pruebaMongo = await _notasRepository.GetAllNotes();

        // throw BusinessException.Throw(_settings,BusinessExceptionTypes.NotControlledException.ToString());

        IEnumerable<Notes> simplifiedNoteResponse = await _notesRepository.GetAllAsync()
                            ?? throw BusinessException.Throw(_settings, BusinessExceptionTypes.NotControlledException.ToString());
        return _mapper.Map<IEnumerable<Notes>, IEnumerable<SimplifiedNoteOutput>>(simplifiedNoteResponse);
    }
    public async Task<NoteOutput> GetNote(string noteId)
    {
        await checkIfNoteExist(noteId);
        var noteResponse = await _notesRepository.FindByIdAsync(noteId)
                        ?? throw BusinessException.Throw(_settings, BusinessExceptionTypes.NotControlledException.ToString());
        return _mapper.Map<Notes, NoteOutput>(noteResponse);
    }
    public async Task<bool> DeleteCheckedIsolateNotes()
    {
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync(x => x.State == NoteStates.CHECKED
            && x.ListId.Trim() == "");
        foreach (var item in notesResponse)
        {
            _notesRepository.Delete(item.Id);
        }
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> ChangeNoteState(string noteId, NoteStateInput data)
    {
        await checkIfNoteExist(noteId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(noteId);
        notesResponse.State = data.State;
        _notesRepository.Update(notesResponse);
        await _unitWork.SaveAsync();
        return true;
    }
    public async Task<bool> UpdateNote(string noteId, ModifiedNoteInput data)
    {
        await checkIfNoteExist(noteId);
        Notes notesResponse = await _notesRepository.FindByIdAsync(noteId);
        notesResponse.Title = string.IsNullOrEmpty(data.Title) ? notesResponse.Title : data.Title;
        notesResponse.State = data.State == 0 ? notesResponse.State : (NoteStates)data.State;
        notesResponse.LastUpdateDate = DateTime.UtcNow;
        notesResponse.UpdaterUser = "Test";
        _notesRepository.Update(notesResponse);
        await _unitWork.SaveAsync();
        return true;
    }
    public void Notification()
    {
        _ = _NotificationEventAdapter.GetNotificationSms();
    }
    #region private
    private async Task checkIfNoteExist(string id)
    {
        Notes result = await _notesRepository.FindByIdAsync(id)
            ?? throw BusinessException.Throw(_settings, BusinessExceptionTypes.NotControlledException.ToString());
    }
    #endregion private
}
