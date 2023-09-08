using Application.DTOs.Notes;
using Core.Enumerations;

namespace Application.Tests.DTOs.Notes;
public static class NoteStateInputStatic
{
    public static NoteStateInput GetNoteStateInput() => new()
    {
        State = NoteStates.CHECKED
    };

}
