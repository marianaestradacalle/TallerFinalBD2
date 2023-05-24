using Application.DTOs.Notes;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    INotesUseCase _notesUseCases;
    public NotesController(INotesUseCase notesUseCases)
    {
        _notesUseCases = notesUseCases;
    }

    [HttpPost("post")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Post([FromBody] NoteInput body)
    {
        return Ok(await _notesUseCases.CreateNote(body));
    }

    [HttpGet("getall")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _notesUseCases.GetNotes());
    }

    [HttpGet("getone")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetOne([FromQuery] string id)
    {
        return Ok(await _notesUseCases.GetNote(id));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        await _notesUseCases.DeleteNote(id);
        return Ok();
    }

    [HttpDelete("checked-isolate-notes")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteChecked()
    {
        await _notesUseCases.DeleteCheckedIsolateNotes();
        return Ok();
    }

    [HttpPost("notes/states")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ChangeState([FromQuery] string id, [FromBody] NoteStateInput body)
    {
        await _notesUseCases.ChangeNoteState(id, body);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Patch([FromQuery] string id, [FromBody] ModifiedNoteInput body)
    {
        await _notesUseCases.UpdateNote(id, body);
        return Ok();
    }
}