using Application.DTOs.Notes;
using Core.Entities;
using Core.Enumerations;
using FluentAssertions;
using System;
using Xunit;

namespace Core.Tests.Entities;
public class NotesTests
{
    [Theory]
    [InlineData("test")]
    public void Notes_AsignaElValorALaPropiedad(string title)
    {
        // Arrange
        string id = Guid.Parse("fe78c26b-a636-4d54-b53b-2d118411d6d7").ToString();
        DateTime creationDate = DateTime.Parse("2023/03/13 16:00:00");
        DateTime lastUpdateDate = DateTime.Parse("2023/03/13 16:05:06");
        NoteStates state = NoteStates.CHECKED;
        string creatorUser = "wponce";
        string pdaterUser = "altorres";
        string idNoteList = "";

        // Act
        var result = new Notes(
            id,
            title,
            state,
            creationDate,
            lastUpdateDate,
            creatorUser,
            pdaterUser,
            idNoteList);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(Guid.Parse("fe78c26b-a636-4d54-b53b-2d118411d6d7").ToString());
        result.Title.Should().Be("test");
        result.State.Should().Be(NoteStates.CHECKED);
        result.CreationDate.Should().Be(DateTime.Parse("2023/03/13 16:00:00"));
        result.CreatorUser.Should().Be("wponce");
        result.UpdaterUser.Should().Be("altorres");
    }
}
