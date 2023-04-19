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

namespace Application.Services.Compound;
public class NoteCleaningService : INoteCleaningService
{
    private readonly IGenericRepositoryService<NoteLists> _noteListRepository;
    private readonly ILogger<NoteCleaningService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepositoryService<Notes> _notesRepository;
    private readonly IUnitWork _unitWork;
    private readonly BusinessSettings _settings;

    public NoteCleaningService(IGenericRepositoryService<NoteLists> notaListRepository,
        ILogger<NoteCleaningService> logger,
        IMapper mapper,
        IGenericRepositoryService<Notes> notesRepository,
        IUnitWork unitWork,
        IOptionsMonitor<BusinessSettings> settings)
    {
        _noteListRepository = notaListRepository;
        _logger = logger;
        _mapper = mapper;
        _notesRepository = notesRepository;
        _unitWork = unitWork;
        _settings = settings.CurrentValue;
    }

    public async Task DeleteAllChecked()
    {
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync(x => x.State == NoteStates.CHECKED)
                    ?? throw BusinessException.Throw(BusinessExceptionTypes.RecordNotFound, _settings);

        IEnumerable<NoteLists> notesListResponse = (IEnumerable<NoteLists>)await _noteListRepository.GetAllAsync(x => x.State == NoteStates.CHECKED)
            ?? throw BusinessException.Throw(BusinessExceptionTypes.RecordNotFound, _settings);
        foreach (var item in notesResponse)
        {
            _notesRepository.Delete(item.Id);
        }
        foreach (var item in notesListResponse)
        {
            _noteListRepository.Delete(item.Id);
        }
        await _unitWork.SaveAsync();
    }
    public async Task InitializationWithList()
    {
        NoteListInput data = new();
        data.Name = "ListInitial";
        await _noteListRepository.AddAsync(_mapper.Map<NoteListInput, NoteLists>(data));
        await _unitWork.SaveAsync();
    }
}
