using Application.DTOs.NotesLists;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[ApiController]
[Route("api/lists")]
public class NoteListsController : ControllerBase
{
    INoteListsUseCase _listsUseCases;
    public NoteListsController(INoteListsUseCase listsUseCases)
    {
        _listsUseCases = listsUseCases;
    }

    [HttpPost("post")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Post([FromBody] NoteListInput body)
    {
        return Ok(await _listsUseCases.CreateNoteList(body));
    }

    [HttpGet("getall")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _listsUseCases.GetNoteLists());
    }

    [HttpGet("getone")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetOne([FromQuery] string id)
    {
        return Ok(await _listsUseCases.GetNoteList(id));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        await _listsUseCases.DeleteNoteList(id);
        return Ok();
    }

    [HttpDelete("checked-lists")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteChecked()
    {
        await _listsUseCases.DeleteCheckedNoteLists();
        return Ok();
    }

    [HttpPost("lists/notes")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddNote([FromQuery] string id, [FromBody] RelatedNoteInput body)
    {
        await _listsUseCases.AddNoteToList(id, body);
        return Ok();
    }

    [HttpDelete("lists/notes")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RemoveNote([FromQuery] string listId, [FromQuery] string noteId)
    {
        await _listsUseCases.RemoveNoteFromList(listId, noteId);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Patch([FromQuery] string listId, [FromBody] ModifiedNoteListsInput body)
    {
        await _listsUseCases.UpdateNoteLists(listId, body);
        return Ok();
    }

    [HttpPost("CheckedNoteLists")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CheckedNoteLists()
    {
        await _listsUseCases.CheckedNoteLists();
        return Ok();
    }
}