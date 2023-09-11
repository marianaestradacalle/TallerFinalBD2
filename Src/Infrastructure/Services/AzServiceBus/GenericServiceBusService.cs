using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;

namespace Services.AzServiceBus;
public class GenericServiceBusService<T> : IGenericServiceEventAdapter<T> where T : class
{   
    private readonly IDirectAsyncGateway<T> _directAsyncGateway;
    private readonly ILogger<GenericServiceBusService<T>> _logger;
    private bool disposedValue;

    public GenericServiceBusService(
            IDirectAsyncGateway<T> directAsyncGateway,
            ILogger<GenericServiceBusService<T>> logger)
    {
        _directAsyncGateway = directAsyncGateway;
        _logger = logger;
    }
    public async Task SendCommandQueue(T data, string eventName, string requestQueue, string id)
    {
        try
        {
            _logger.LogInformation("Se inicia envío comando a la cola {@requestQueue}, data: {@data}", requestQueue, data);
            await _directAsyncGateway.SendCommand(requestQueue, new Command<T>(name: eventName, commandId: id, data: data));
            _logger.LogInformation("Se finaliza envío comando a la cola {@requestQueue}, data: {@data}", requestQueue, data);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public Task SendEventTopic(T data, string eventName, string requestTopic, string id)
    {
        return Task.Run(() =>
        {
            _logger.LogInformation("Se inicia envío evento al tópico {@requestTopic}, data: {@data}", requestTopic, data);
            _directAsyncGateway.SendEvent(requestTopic, new DomainEvent<T>(eventName, id, data));
            _logger.LogInformation("Se finaliza envío evento al tópico {@requestTopic}, data: {@data}", requestTopic, data);
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {           
            disposedValue = true;
        }
    }

    public void Dispose()
    {        
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
