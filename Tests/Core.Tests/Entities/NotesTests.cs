using Core.Entities;
using Core.Enumerations;
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
        Assert.NotNull(result);
        Assert.True(result.Id.Equals(Guid.Parse("fe78c26b-a636-4d54-b53b-2d118411d6d7").ToString()));
        Assert.True(result.LastUpdateDate.Equals(DateTime.Parse("2023/03/13 16:05:06")));
        Assert.True(result.Title.Equals("test"));
        Assert.True(result.State.Equals(NoteStates.CHECKED));
        Assert.True(result.CreationDate.Equals(DateTime.Parse("2023/03/13 16:00:00")));
        Assert.True(result.CreatorUser.Equals("wponce"));
        Assert.True(result.UpdaterUser.Equals("altorres"));
    }
}
