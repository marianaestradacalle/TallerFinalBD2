using Core.Enumerations;

namespace Core.Entities;
public class Notes
{
    public string Id { get; set; }
    public string Title { get; set; }
    public NoteStates State { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string CreatorUser { get; set; }
    public string UpdaterUser { get; set; }
    public string ListId { get; set; }

    public Notes(string id, string title, NoteStates state, DateTime creationDate,
       DateTime? lastUpdateDate, string creatorUser, string updaterUser, string listId)
    {
        Id = id;
        Title = title;
        State = state;
        CreationDate = creationDate;
        LastUpdateDate = lastUpdateDate;
        CreatorUser = creatorUser;
        UpdaterUser = updaterUser;
        ListId = listId;

    }
}