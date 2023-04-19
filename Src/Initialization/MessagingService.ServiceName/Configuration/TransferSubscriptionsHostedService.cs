using MessagingService.ServiceName.Subscriptions;

namespace MessagingService.ServiceName.Configuration;
public class TransferSubscriptionsHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TransferSubscriptionsHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IServiceScope scope = _serviceScopeFactory.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;

        INotesSubscription notesSubscription = serviceProvider.GetService<INotesSubscription>();
        await notesSubscription.SubscribeAsync();
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
