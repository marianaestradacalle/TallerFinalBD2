using Application.DTOs.NotesLists;

namespace Dobles.Tests.Application.DTOs.Tests.NoteLists;
public static class NoteListInputStatic
{
    public static NoteListInput GetNoteNoteListInput() => new()
    {
        Name = "test"
    };

}
