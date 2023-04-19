using Hangfire;
using Core.Enumerations;
using Application.Common.Utilities;

namespace CronJobService.ServiceName.Configuration;

public static class ConfigurationJobs
{
    public static IServiceProvider AddJobs(this IServiceProvider ServiceProvider, BusinessSettings settings,
        IEnumerable<ICreatorNotesJob> creatorNotesJob)
    {
        foreach (ICreatorNotesJob job in creatorNotesJob)
        {
            var jobValues = GetJobValues(job.jobIdentifier, settings);
            RecurringJob.AddOrUpdate(jobValues.Item1, () => job.Invoke(), jobValues.Item2);
        }
        return ServiceProvider;
    }
    private static Tuple<string, string> GetJobValues(JobIdentifier jobIdentifier, BusinessSettings settings)
    {
        string name = jobIdentifier.ToString();
        switch (jobIdentifier)
        {
            default:
            case JobIdentifier.FirstTask:
                return Tuple.Create(name, settings.CRON_SONDA_JOB);
        }
    }
}
