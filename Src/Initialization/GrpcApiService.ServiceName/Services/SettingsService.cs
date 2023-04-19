using Application.Interfaces.Services;
using Grpc.Core;
using GrpcSettingsService;

namespace GrpcApiService.ServiceName.Services;
public class SettingsService : GrpcSettingsService.SettingsService.SettingsServiceBase
{
    private readonly INoteCleaningService _noteCleaningService;
    private readonly IClearAllService _clearAllUseCase;
    
    public SettingsService(INoteCleaningService noteCleaningService,
            IClearAllService clearAllUseCase)
    {
        _noteCleaningService = noteCleaningService;
        _clearAllUseCase = clearAllUseCase;
    }
    public override async Task<Empty> DeleteCompletedItems(Empty request,
    ServerCallContext context)
    {
        _ = _noteCleaningService.DeleteAllChecked();
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> DeleteAllItems(Empty request,
    ServerCallContext context)
    {
        _ = _clearAllUseCase.Apply();
        Empty response = new Empty();
        return response;
    }

    public override async Task<Empty> PostCanvas(Empty request, ServerCallContext context)
    {
        _ = _noteCleaningService.InitializationWithList();
        Empty response = new Empty();
        return response;
    }

}
