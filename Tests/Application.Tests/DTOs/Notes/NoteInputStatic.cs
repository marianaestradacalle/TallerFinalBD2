using Application.DTOs.Notes;

namespace Application.Tests.DTOs.Notes;
public static class NoteInputStatic
{
    public static NoteInput GetNoteInput() => new()
    {
        Title = "test"
    };

}
