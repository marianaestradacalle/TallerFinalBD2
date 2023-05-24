namespace Application.Interfaces.Infraestructure;
public interface IGenericServiceEventAdapter<T> : IDisposable where T : class
{
    Task SendCommandQueue(T data, string eventName, string requestQueue, string id);
    Task SendEventTopic(T data, string eventName, string requestTopic, string id);

}
