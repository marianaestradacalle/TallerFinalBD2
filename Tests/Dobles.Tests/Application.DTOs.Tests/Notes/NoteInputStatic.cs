using Application.DTOs.Notes;

namespace Dobles.Tests.Application.DTOs.Tests.Notes;
public static class NoteInputStatic
{
    public static NoteInput GetNoteInput() => new()
    {
        Title = "test"
    };

}
