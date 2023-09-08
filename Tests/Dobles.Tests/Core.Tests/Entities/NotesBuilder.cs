using Core.Entities;
using Core.Enumerations;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Dobles.Tests.Core.Tests.Entities;
public class NotesBuilder
{
    //Arrange
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

    public Notes Notes()=> 
       new Notes(_id, _title,_state, _creationDate,_lastUpdateDate, _creatorUser,_updaterUser,_idNoteList);

    [Theory]
    [InlineData("test")]
    public void Notes_AsignaElValorALaPropiedad(string title)
    {

        // Act
        var result = new Notes(_id, title, _state, _creationDate, _lastUpdateDate, _creatorUser, _updaterUser, _idNoteList);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_id);
        result.Title.Should().Be("test");
        result.State.Should().Be(NoteStates.CHECKED);
        result.CreationDate.Should().Be(DateTime.Parse("2023/03/13 16:00:00"));
        result.LastUpdateDate.Should().Be(_lastUpdateDate);
        result.CreatorUser.Should().Be("altorres");
        result.UpdaterUser.Should().Be("wponce");

    }
}
