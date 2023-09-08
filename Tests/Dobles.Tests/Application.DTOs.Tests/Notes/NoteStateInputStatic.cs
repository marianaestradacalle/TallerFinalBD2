using Application.DTOs.Notes;
using Core.Enumerations;

namespace Dobles.Tests.Application.DTOs.Tests.Notes;
public static class NoteStateInputStatic
{
    public static NoteStateInput GetNoteStateInput() => new()
    {
        State = NoteStates.CHECKED
    };

}
