using Application.Interfaces.Infraestructure;

namespace Services.AzServiceBus;
public class NotificationServiceBusService : INotificationServiceBusService
{
    private readonly IGenericServiceBusService<object> _genericEventAdapter;
    public NotificationServiceBusService(IGenericServiceBusService<object> genericEventAdapter)
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
