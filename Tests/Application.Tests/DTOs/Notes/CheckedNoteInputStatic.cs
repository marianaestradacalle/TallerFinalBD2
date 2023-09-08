using Application.DTOs.Notes;
using System;

namespace Application.Tests.DTOs.Notes;
public static class CheckedNoteInputStatic
{
    public static CheckedNoteInput GetCheckedNoteInput() => new()
    {
        Id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString(),
        State = 0
    };

}
