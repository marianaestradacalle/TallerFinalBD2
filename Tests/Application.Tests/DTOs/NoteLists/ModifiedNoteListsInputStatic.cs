using Application.DTOs.NotesLists;

namespace Application.Tests.DTOs.Notes;
public static class ModifiedNoteListsInputStatic
{
    public static ModifiedNoteListsInput GetModifiedNoteListsInput() => new()
    {
        Name = "test",
        State=0
    };

}
