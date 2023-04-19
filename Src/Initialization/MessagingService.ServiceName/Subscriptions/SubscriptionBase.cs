using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MessagingService.ServiceName.Subscriptions;
public class SubscriptionBase<T>
{
    protected async Task SubscribeOnCommandAsync<TEntity>(IDirectAsyncGateway<TEntity> directAsyncGateway,
        string targetName, Func<Command<TEntity>, Task> handler, MethodBase methodBase, int maxConcurrentCalls = 1,
        [CallerMemberName] string callerMemberName = null)
    {
        try
        {
            await directAsyncGateway.SuscripcionCommand(targetName, handler, maxConcurrentCalls);
        }
        catch (Exception ex)
        {

        }
    }
    public async Task SubscribeOnEventAsync<T>(IDirectAsyncGateway<T> directAsyncGateway,
        string targetName, string subscriptionName, Func<DomainEvent<T>, Task> handler, MethodBase methodBase, int maxConcurrentCalls = 1,
        [CallerMemberName] string callerMemberName = null)
    {
        try
        {
            await directAsyncGateway.SuscripcionEvent(targetName, subscriptionName, handler, maxConcurrentCalls);
        }
        catch (Exception ex)
        {


        }
    }
    protected async Task HandleCommandAsync<TEntity>(Func<TEntity, Task> serviceHandler, MethodBase methodBase, string logId, Command<TEntity> request,
        bool notifyBusinessException = false, [CallerMemberName] string callerMemberName = null) =>
        await HandleRequestAsync(async () =>
        {
            Validate(request);
            return await InvokeAsync(async (entity) =>
            {
                await serviceHandler(entity);
                return true;
            },
            request.data);
        },
        methodBase,
        logId,
        request,
        callerMemberName,
        notifyBusinessException);
    public async Task HandleEventAsync<TEntity>(Func<TEntity, Task> serviceHandler, MethodBase methodBase, string logId, DomainEvent<TEntity> request,
            bool notifyBusinessException = false, [CallerMemberName] string callerMemberName = null) =>
            await HandleRequestAsync(async () =>
            {

                if (request == null || request.data == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                return await InvokeAsync(async (entity) =>
                {
                    await serviceHandler(entity);

                    return true;
                },
                    request.data);
            },
            methodBase,
            logId,
            request,
            callerMemberName,
            notifyBusinessException);

    private async Task<TResult> HandleRequestAsync<TResult, TRequest>(Func<Task<TResult>> processMessage, MethodBase methodBase,
        string logId, TRequest request, string callerMemberName, bool notifyBusinessException = false)
    {
        try
        {
            return await processMessage();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private async Task<TResult> InvokeAsync<TEntity, TResult>(Func<TEntity, Task<TResult>> handler, TEntity entity)
    {
        return await handler(entity);
    }
    public void Validate<TEntity>(Command<TEntity> command)
    {
        if (command == null || command.data == null)
        {
            throw new ArgumentNullException(nameof(command));
        }
    }
}
