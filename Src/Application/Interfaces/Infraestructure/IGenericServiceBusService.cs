namespace Application.Interfaces.Infraestructure;
public interface IGenericServiceBusService<T> : IDisposable where T : class
{
    Task SendCommandQueue(T data, string eventName, string requestQueue, string id);
    Task SendEventTopic(T data, string eventName, string requestTopic, string id);

}
