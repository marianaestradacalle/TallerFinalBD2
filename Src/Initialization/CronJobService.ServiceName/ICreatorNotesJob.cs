using Core.Enumerations;

namespace CronJobService.ServiceName;
public interface ICreatorNotesJob
{
    Task Invoke();
    public JobIdentifier jobIdentifier { get; }
}
