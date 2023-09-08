using Application.DTOs.Notes;

namespace Application.Tests.DTOs.Notes;
public static class ModifiedNoteInputStatic
{
    public static ModifiedNoteInput GetModifiedNoteInput() => new()
    {
        Title = "test",
        State = 0
    };

}
