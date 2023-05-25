using Application.Interfaces.Infraestructure;

namespace Services.AzServiceBus;
public class NotificationServiceBusService : INotificationServiceEventAdapter
{
    private readonly IGenericServiceEventAdapter<object> _genericEventAdapter;
    public NotificationServiceBusService(IGenericServiceEventAdapter<object> genericEventAdapter)
    {
        _genericEventAdapter = genericEventAdapter;
    }
    public async Task GetNotificationSms()
    {
        string commandName = "notifications_send_sms";
        var dataResend = new
        {
            Mobile = "3000000000",
            Message = $"{"Prueba notificacion al mobile: test."}"
        };
        await _genericEventAdapter.SendCommandQueue(dataResend, commandName, commandName, "1");
    }
}
