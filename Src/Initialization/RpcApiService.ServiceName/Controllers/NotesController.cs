using Application.DTOs.Notes;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoints.RpcApi.Controllers;

[ApiController]
[Route("api/notes/")]
public class NotesController : ControllerBase
{
    INotesUseCase _notesUseCases;
    public NotesController(INotesUseCase notesUseCases)
    {
        _notesUseCases = notesUseCases;
    }
    [ActionName("save")]
    [Route("save")]
    [HttpPost]
    public async Task<NoteOutput> Save(NoteInput body)
    {
        return await _notesUseCases.CreateNote(body);

    }
    [ActionName("getAll")]
    [Route("getAll")]
    [HttpGet]
    public async Task<IEnumerable<SimplifiedNoteOutput>> GetAll()
    {         
        return await _notesUseCases.GetNotes();
    }
   
}