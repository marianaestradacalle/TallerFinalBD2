using Application.Common.Utilities;
using Dobles.Tests.Application.Common.Tests.Utilities;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;

namespace Dobles.Tests.Stubs.Generic;
public class StubSettings
{
    private readonly Mock<IOptionsMonitor<BusinessSettings>> _appSettings = new();
    
    public IOptionsMonitor<BusinessSettings> StubSetting()
    {
        _appSettings.Setup(settings => settings.CurrentValue).Returns(new AppSettingsBuilder()
            .WithServiceExceptions(new List<ServiceException>
            {
                new ServiceExceptionBuilder().WithId("RecordNotFound").WithCode("409.006").WithMessage("No existe el registro.").Build(),
            })
            .Build());
        return _appSettings.Object;
    }

}
