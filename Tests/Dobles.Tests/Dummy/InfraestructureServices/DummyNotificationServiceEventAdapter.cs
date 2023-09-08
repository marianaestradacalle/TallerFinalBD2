using Application.Interfaces.Infraestructure;
using System;
using System.Threading.Tasks;

namespace Dobles.Tests.Dummy.InfraestructureServices
{
    public class DummyNotificationServiceEventAdapter : INotificationServiceEventAdapter
    {
        public async Task GetNotificationSms()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Stub notificacion sms");
            });
        }
    }
}
