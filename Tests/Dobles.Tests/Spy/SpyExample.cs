using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System;
using Xunit;

namespace Dobles.Tests.Spy;
public class SpyExample
{
    private Mock<ILogger> _logger;

    private List<String> spyList = new List<String>();
    [Fact]
    public void ModuleThrowExceptionInvokesLoggerOnlyOnce()
    {
        spyList.Add("Test");
        Assert.Equal("Test", spyList[0]);
        _logger = new Mock<ILogger>();
        _logger.Setup(x => x.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>())).Verifiable();

    }
}
