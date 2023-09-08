using Application.DTOs.Notes;

namespace Dobles.Tests.Application.DTOs.Tests.Notes;
public static class ModifiedNoteInputStatic
{
    public static ModifiedNoteInput GetModifiedNoteInput() => new()
    {
        Title = "test",
        State = 0
    };

}
