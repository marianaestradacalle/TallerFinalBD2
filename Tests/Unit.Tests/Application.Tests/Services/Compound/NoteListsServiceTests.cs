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

namespace Unit.Tests.Application.Tests.Services.Compound;
public class NoteListsServiceTests
{
    private StubSettings stubSettings = new StubSettings();
    private DummyNotificationServiceEventAdapter dummyNotificationServiceEventAdapter = new DummyNotificationServiceEventAdapter();
    private DummyUnitWork dummyUnitWork = new DummyUnitWork();
    private FakeGenericRepositoryServiceNotes<Notes> fakeGenericRepositoryService = new FakeGenericRepositoryServiceNotes<Notes>();
    private MockConfiguration<NotesService> mockConfiguration = new MockConfiguration<NotesService>();
    public NotesService CreateNotesService;
    private FakeMapper fakeMapper = new FakeMapper();
    private FakeNotasRepository fakeNotasRepository = new FakeNotasRepository();

    public NoteListsServiceTests() 
    {
        
         CreateNotesService= new NotesService(fakeGenericRepositoryService,
                                    (ILogger<NotesService>)mockConfiguration.MockLogger(),
                                    (IMapper)fakeMapper.GetFakeMapper(),
                                    dummyNotificationServiceEventAdapter,
                                    stubSettings.StubSetting(),
                                    dummyUnitWork,
                                    fakeNotasRepository);
    }    


}