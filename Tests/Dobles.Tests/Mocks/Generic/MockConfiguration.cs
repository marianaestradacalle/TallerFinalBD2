using Microsoft.Extensions.Logging;
using Moq;

namespace Dobles.Tests.Mocks.Generic;
public class MockConfiguration<Entity>
{    
    public object MockLogger()
    {
        var mock = new Mock<ILogger<Entity>>();
        return mock.Object;
    }
}
