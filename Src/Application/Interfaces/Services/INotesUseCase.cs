using Application.DTOs.Notes;

namespace Application.Interfaces.Services;
public interface INotesUseCase
{
    public Task<NoteOutput> CreateNote(NoteInput data);
    public Task<IEnumerable<SimplifiedNoteOutput>> GetNotes();
    public Task<NoteOutput> GetNote(string noteId);
    public Task DeleteNote(string noteId);
    public Task DeleteCheckedIsolateNotes();
    public Task ChangeNoteState(string noteId, NoteStateInput data);
    public Task UpdateNote(string noteId, ModifiedNoteInput data);
    public void Notification();
}
