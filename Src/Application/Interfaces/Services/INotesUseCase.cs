using Application.DTOs.Notes;

namespace Application.Interfaces.Services;
public interface INotesUseCase
{
    public Task<NoteOutput> CreateNote(NoteInput data);
    public Task<IEnumerable<SimplifiedNoteOutput>> GetNotes();
    public Task<NoteOutput> GetNote(string noteId);
    public Task<bool> DeleteNote(string noteId);
    public Task<bool> DeleteCheckedIsolateNotes();
    public Task<bool> ChangeNoteState(string noteId, NoteStateInput data);
    public Task<bool> UpdateNote(string noteId, ModifiedNoteInput data);
    public void Notification();
}
