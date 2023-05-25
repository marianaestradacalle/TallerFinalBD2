using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    INoteCleaningService _noteCleaningService;
    IClearAllUseCase _clearAllUseCase;
    public SettingsController(INoteCleaningService noteCleaningService, IClearAllUseCase clearAllUseCase)
    {
        _noteCleaningService = noteCleaningService;
        _clearAllUseCase = clearAllUseCase;
    }

    [HttpDelete("completed-items")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteCompletedItems()
    {
        await _noteCleaningService.DeleteAllChecked();
        return Ok();
    }

    [HttpDelete("all-items")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteAllItems()
    {
        await _clearAllUseCase.Apply();
        return Ok();
    }

    [HttpPost("canvas")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> PostCanvas()
    {
        await _noteCleaningService.InitializationWithList();
        return Ok();
    }
}