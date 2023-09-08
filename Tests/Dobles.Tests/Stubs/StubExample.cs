using Application.Interfaces.Services;
using Moq;
using Xunit;

namespace Dobles.Tests.Stubs;
public class StubExample
{
    [Fact]
    public void PlayerRollDieWithMaxFaceValue()
    {
       var stubDie = new Mock<INotesUseCase>();
       INotesUseCase die = stubDie.Object;
       
    }
}
