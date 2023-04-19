using Core.Enumerations;

namespace Core.Entities;
public class NoteLists
{
    public string Id { get; set; }
    public string Name { get; set; }
    public NoteStates State { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string CreatorUser { get; set; }
    public string UpdaterUser { get; set; }

    public NoteLists(string id, string name, NoteStates state, DateTime creationDate,
       DateTime? lastUpdateDate, string creatorUser, string updaterUser)
    {
        Id = id;
        Name = name;
        State = state;
        CreationDate = creationDate;
        LastUpdateDate = lastUpdateDate;
        CreatorUser = creatorUser;
        UpdaterUser = updaterUser;

    }
}

