using Application.DTOs.NotesLists;
using System;

namespace Application.Tests.DTOs.Notes;
public static class RelatedNoteInputStatic
{
    public static RelatedNoteInput GetRelatedNoteInput() => new()
    {
        NoteId = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString()
    };

}
