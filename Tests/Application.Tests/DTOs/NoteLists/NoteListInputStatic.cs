using Application.DTOs.NotesLists;

namespace Application.Tests.DTOs.Notes;
public static class NoteListInputStatic
{
    public static NoteListInput GetNoteNoteListInput() => new()
    {
        Name = "test"
    };

}
