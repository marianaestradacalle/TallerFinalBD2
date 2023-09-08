using Application.DTOs.NotesLists;

namespace Dobles.Tests.Application.DTOs.Tests.NoteLists;
public static class ModifiedNoteListsInputStatic
{
    public static ModifiedNoteListsInput GetModifiedNoteListsInput() => new()
    {
        Name = "test",
        State = 0
    };

}
