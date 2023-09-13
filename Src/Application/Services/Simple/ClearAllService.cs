using AutoMapper;
using Common.Helpers.Exceptions;
using Application.Common.Utilities;
using Core.Entities;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Simple;
public class ClearAllService : IClearAllUseCase
{
    private readonly IGenericRepositoryAdapter<NoteLists> _notaListRepository;
    private readonly ILogger<ClearAllService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepositoryAdapter<Notes> _notesRepository;
    private readonly IUnitWork _unitWork;
    private readonly BusinessSettings _settings;

    public ClearAllService(IGenericRepositoryAdapter<NoteLists> notaListRepository,
        ILogger<ClearAllService> logger,
        IMapper mapper,
        INotesUseCase notesUseCases,
        IGenericRepositoryAdapter<Notes> notesRepository,
        IUnitWork unitWork,
        IOptionsMonitor<BusinessSettings> settings)
    {
        _notaListRepository = notaListRepository;
        _logger = logger;
        _mapper = mapper;
        _notesRepository = notesRepository;
        _unitWork = unitWork;
        _settings = settings.CurrentValue;
    }

    public async Task<bool> Apply()
    {
        IEnumerable<Notes> notesResponse = (IEnumerable<Notes>)await _notesRepository.GetAllAsync()
                        ?? throw BusinessException.Throw(_settings, BusinessExceptionTypes.NotControlledException.ToString());

        IEnumerable<NoteLists> notesListResponse = (IEnumerable<NoteLists>)await _notaListRepository.GetAllAsync()
                    ?? throw BusinessException.Throw(_settings, BusinessExceptionTypes.NotControlledException.ToString());
        foreach (var item in notesResponse)
        {
            _notesRepository.Delete(item.Id);
        }
        foreach (var item in notesListResponse)
        {
            _notaListRepository.Delete(item.Id);
        }
        await _unitWork.SaveAsync();
        return true;
    }
}
