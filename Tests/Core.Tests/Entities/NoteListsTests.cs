using Core.Entities;
using Core.Enumerations;
using FluentAssertions;
using System;
using Xunit;

namespace Core.Tests.Entities;
public class NoteListsTests
{
    [Theory]
    [InlineData("test")]
    public void NoteLists_AsignaElValorALaPropiedad(string name)
    {
        // Arrange
        string id = Guid.Parse("fe78c26b-a636-4d54-b53b-2d118411d6d7").ToString();
        DateTime creationDate = DateTime.Parse("2023/03/13 16:00:00");
        DateTime lastUpdateDate = DateTime.Parse("2023/03/13 16:05:06");
        NoteStates state = NoteStates.CHECKED;
        string creatorUser = "wponce";
        string pdaterUser = "altorres";
        
        // Act
        var result = new NoteLists(
            id,
            name,
            state,
            creationDate,
            lastUpdateDate,
            creatorUser,
            pdaterUser);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(Guid.Parse("fe78c26b-a636-4d54-b53b-2d118411d6d7").ToString());
        result.Name.Should().Be("test");
        result.State.Should().Be(NoteStates.CHECKED);
        result.CreationDate.Should().Be(DateTime.Parse("2023/03/13 16:00:00"));
        result.CreatorUser.Should().Be("wponce");
        result.UpdaterUser.Should().Be("altorres");
    }
}
