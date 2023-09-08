using Core.Entities;
using Core.Enumerations;
using System;

namespace Core.Tests.Entities;
public class NoteListsBuilder
{
    private string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
    private string _name = "test";
    private DateTime? _lastUpdateDate = DateTime.Parse("2023/03/13 16:05:06");
    private NoteStates _state = NoteStates.CHECKED;
    private DateTime _creationDate = DateTime.Parse("2023/03/13 16:00:00");
    private string _creatorUser = "altorres";
    private string _updaterUser = "wponce";
        
    public NoteListsBuilder(){ }

    public NoteListsBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public NoteListsBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public NoteListsBuilder WithState(NoteStates state)
    {
        _state = state;
        return this;
    }

    public NoteListsBuilder WithCreationDate(DateTime creationDate)
    {
        _creationDate = creationDate;
        return this;
    }

    public NoteListsBuilder WithLastUpdateDate(DateTime? lastUpdateDate)
    {
        _lastUpdateDate = lastUpdateDate;
        return this;
    }

    public NoteListsBuilder WithCreatorUser(string creatorUser)
    {
        _creatorUser = creatorUser;
        return this;
    }

    public NoteListsBuilder WithUpdaterUser(string updaterUser)
    {
        _updaterUser = updaterUser;
        return this;
    }


    public NoteLists NoteLists()=> new NoteLists(_id, _name ,_state, _creationDate,_lastUpdateDate, _creatorUser,_updaterUser);

}
