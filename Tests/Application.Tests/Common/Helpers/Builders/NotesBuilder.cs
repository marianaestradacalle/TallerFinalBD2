using Core.Entities;
using Core.Enumerations;
using System;

namespace Application.Common.Tests.Helpers.Builders;
public class NotesBuilder
{
    private string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
    private string _title = "test";
    private DateTime? _lastUpdateDate = DateTime.Parse("2023/03/13 16:05:06");
    private NoteStates _state = NoteStates.CHECKED;
    private DateTime _creationDate = DateTime.Parse("2023/03/13 16:00:00");
    private string _creatorUser = "altorres";
    private string _updaterUser = "wponce";
    private string _idNoteList = "";
    
    public NotesBuilder(){ }

    public NotesBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public NotesBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public NotesBuilder WithState(NoteStates state)
    {
        _state = state;
        return this;
    }

    public NotesBuilder WithCreationDate(DateTime creationDate)
    {
        _creationDate = creationDate;
        return this;
    }

    public NotesBuilder WithLastUpdateDate(DateTime? lastUpdateDate)
    {
        _lastUpdateDate = lastUpdateDate;
        return this;
    }

    public NotesBuilder WithCreatorUser(string creatorUser)
    {
        _creatorUser = creatorUser;
        return this;
    }

    public NotesBuilder WithUpdaterUser(string updaterUser)
    {
        _updaterUser = updaterUser;
        return this;
    }

    public NotesBuilder WithIdNoteList(string idNoteList)
    {
        _idNoteList = idNoteList;
        return this;
    }

    public Notes Notes()=> new Notes(_id, _title,_state, _creationDate,_lastUpdateDate, _creatorUser,_updaterUser,_idNoteList);

}
