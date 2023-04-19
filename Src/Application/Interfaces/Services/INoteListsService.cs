using Application.DTOs.NotesLists;

namespace Application.Interfaces.Services;
public interface INoteListsService
{
    public Task<NoteListOutput> CreateNoteList(NoteListInput data);
    public Task<IEnumerable<SimplifiedNoteListOutput>> GetNoteLists();
    public Task<NoteListOutput> GetNoteList(string listId);
    public Task DeleteNoteList(string listId);
    public Task DeleteCheckedNoteLists();
    public Task AddNoteToList(string listId, RelatedNoteInput data);
    public Task RemoveNoteFromList(string listId, string noteId);
    public Task CheckedNoteLists();
    public Task UpdateNoteLists(string listId, ModifiedNoteListsInput data);
}
