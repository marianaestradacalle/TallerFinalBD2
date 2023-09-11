using Application.Interfaces.Services;
using Application.Services.Simple;
using AutoMapper;
using Core.Entities;
using Dobles.Tests.Dummy.Generic;
using Dobles.Tests.Dummy.InfrastructureServices;
using Dobles.Tests.Fake.Generic;
using Dobles.Tests.Fake.InfrastructureServices;
using Dobles.Tests.Mocks.Generic;
using Dobles.Tests.Stubs.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit.Tests.Application.Tests.Services.Simple;
public class ClearAllServiceTests
{
    private StubSettings stubSettings = new StubSettings();
    private DummyNotificationServiceEventAdapter dummyNotificationServiceEventAdapter = new DummyNotificationServiceEventAdapter();
    private DummyUnitWork dummyUnitWork = new DummyUnitWork();
    private FakeGenericRepositoryServiceNotes<NoteLists> fakeGenericRepositoryService = new FakeGenericRepositoryServiceNotes<NoteLists>();
    private MockConfiguration<ClearAllService> mockConfiguration = new MockConfiguration<ClearAllService>();
    public ClearAllService CreateClearAllService;
    private readonly Mock<INotesUseCase> _notesUseCase = new Mock<INotesUseCase>();
    private FakeGenericRepositoryServiceNotes<Notes> fakeGenericRepositoryNoteService = new FakeGenericRepositoryServiceNotes<Notes>();
    private FakeMapper fakeMapper = new FakeMapper();
    public ClearAllServiceTests() 
    {
        CreateClearAllService = new ClearAllService(fakeGenericRepositoryService,
                                    (ILogger<ClearAllService>)mockConfiguration.MockLogger(),
                                    (IMapper)fakeMapper.GetFakeMapper(),
                                    _notesUseCase.Object,
                                    fakeGenericRepositoryNoteService,                                    
                                    dummyUnitWork,
                                    stubSettings.StubSetting());
    }    

}