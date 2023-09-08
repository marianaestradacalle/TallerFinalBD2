using Application.DTOs.NotesLists;

namespace Application.Interfaces.Services;
public interface INoteListsUseCase
{
    public Task<NoteListOutput> CreateNoteList(NoteListInput data);
    public Task<IEnumerable<SimplifiedNoteListOutput>> GetNoteLists();
    public Task<NoteListOutput> GetNoteList(string listId);
    public Task<bool> DeleteNoteList(string listId);
    public Task<bool> DeleteCheckedNoteLists();
    public Task<bool> AddNoteToList(string listId, RelatedNoteInput data);
    public Task<bool> RemoveNoteFromList(string listId, string noteId);
    public Task<bool> CheckedNoteLists();
    public Task<bool> UpdateNoteLists(string listId, ModifiedNoteListsInput data);
}
