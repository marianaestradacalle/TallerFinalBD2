using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dobles.Tests.Spy;
public class SpyExample
{
    [Fact]
    public void ModuleThrowExceptionInvokesLoggerOnlyOnce()
    {
        var spyLogger = new Mock<ILogger>();
        //Module module = new Module;
        ILogger logger = spyLogger.Object;
        // module.SetLogger(logger);
        // module.ThrowException("Catch me if you can");
        // module.ThrowException("Catch me if you can");

        spyLogger.Verify(m => m.Log(It.IsAny<LogLevel>(),It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
    
    }
}
