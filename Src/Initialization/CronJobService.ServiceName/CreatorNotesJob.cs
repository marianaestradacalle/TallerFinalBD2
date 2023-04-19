using Application.Interfaces.Services;
using Core.Enumerations;

namespace CronJobService.ServiceName;
public class CreatorNotesJob : ICreatorNotesJob
{
    private readonly INotesService _notesUseCases;

    public CreatorNotesJob(
        INotesService notesUseCases)

    {
        _notesUseCases = notesUseCases;
    }

    public JobIdentifier jobIdentifier => JobIdentifier.FirstTask;

    public async Task Invoke()
    {
        var response = await _notesUseCases.CreateNote(new());
        _notesUseCases.Notification();
    }

}
