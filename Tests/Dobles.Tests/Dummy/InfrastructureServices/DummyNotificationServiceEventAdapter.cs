using Application.Interfaces.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Dobles.Tests.Dummy.InfrastructureServices
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
