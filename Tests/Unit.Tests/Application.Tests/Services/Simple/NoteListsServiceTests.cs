using Application.DTOs.NotesLists;
using Application.Interfaces.Services;
using Application.Services.Simple;
using AutoMapper;
using Core.Entities;
using Core.Enumerations;
using Dobles.Tests.Application.DTOs.Tests.NoteLists;
using Dobles.Tests.Dummy.Generic;
using Dobles.Tests.Dummy.InfrastructureServices;
using Dobles.Tests.Fake.Generic;
using Dobles.Tests.Fake.InfrastructureServices;
using Dobles.Tests.Mocks.Generic;
using Dobles.Tests.Stubs.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Unit.Tests.Application.Tests.Services.Simple;
public class NoteListsServiceTests
{
    private StubSettings stubSettings = new StubSettings();
    private DummyNotificationServiceEventAdapter dummyNotificationServiceEventAdapter = new DummyNotificationServiceEventAdapter();
    private DummyUnitWork dummyUnitWork = new DummyUnitWork();
    private FakeGenericRepositoryServiceNotesList<NoteLists> fakeGenericRepositoryService = new FakeGenericRepositoryServiceNotesList<NoteLists>();
    private MockConfiguration<NoteListsService> mockConfiguration = new MockConfiguration<NoteListsService>();
    public NoteListsService CreateNoteListsService;
    private FakeGenericRepositoryServiceNotes<Notes> fakeGenericRepositoryNoteService = new FakeGenericRepositoryServiceNotes<Notes>();
    private readonly Mock<INotesUseCase> _notesUseCase = new Mock<INotesUseCase>();
    private FakeMapper fakeMapper = new FakeMapper();

    public NoteListsServiceTests() 
    {
         CreateNoteListsService= new NoteListsService(fakeGenericRepositoryService,
                                    (ILogger<NoteListsService>)mockConfiguration.MockLogger(),
                                    (IMapper)fakeMapper.GetFakeMapper(),
                                    _notesUseCase.Object,
                                    fakeGenericRepositoryNoteService,                                    
                                    dummyUnitWork,
                                    stubSettings.StubSetting());
    }    


    [Fact]
    public async Task CreateNoteList_ValidRegister_ReturnNoteList()
    {
        // Arrange                 
        NoteListInput noteListInput = NoteListInputStatic.GetNoteNoteListInput();

        // Act
        var result = await CreateNoteListsService.CreateNoteList(noteListInput);       
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoteListOutput>();
        result.Name.Should().Be("test");
        result.State.Should().Be((int)NoteStates.NOTCHECKED);
        result.Id.Should().NotBeEquivalentTo(Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        
        Assert.NotNull(result);
        Assert.IsType<NoteListOutput>(result);
        Assert.NotEqual(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal(result.State, (int)NoteStates.NOTCHECKED);
    }

    [Fact]
    public async Task GetNote_GetValidNoteById_ReturnNote()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();

        // Act
        var result = await CreateNoteListsService.GetNoteList(_id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoteListOutput>();
        result.Name.Should().Be("test");
        result.Id.Should().Be(_id);

        Assert.NotNull(result);
        Assert.IsType<NoteListOutput>(result);
        Assert.Equal(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal("test", result.Name);
        
    }

    [Fact]
    public async Task UpdateNoteLists_ValidUpdateNoteById_ReturnTrue()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        ModifiedNoteListsInput data = ModifiedNoteListsInputStatic.GetModifiedNoteListsInput();

        // Act
        var result = await CreateNoteListsService.UpdateNoteLists(_id, data);        

        // Assert
        result.Should().BeTrue();

        Assert.True(result);

    }

    [Fact]
    public async Task DeleteNote_ValidDeletedNoteById_ReturnTrue()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        string listId; string noteId;
        // Act
      //  await CreateNoteListsService.RemoveNoteFromList(listId, noteId);

        // Assert
        //Assert.Equal(note.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
    }

    [Fact]
    public async Task GetNote_GetInvalidNoteById_ReturnException()
    {
        // Arrange
        string _id = "";

        // Act
        //var result1 = await Assert.ThrowsAnyAsync<BusinessException>(async () => { await CreateNoteListsService.GetNote(_id); });
        //var result = await this.Awaiting(_ => notesUseCases.GetNote(_id)).Should().ThrowAsync<BusinessException>();
       // Func<Task> result = async () => { await CreateNoteListsService.GetNote(_id); };
        
        // Assert        
        //await result.Should().ThrowAsync<BusinessException>();

        //result.Should().Which("No existe el registro.");



        // result.Should().ThrowBusinessException>();


        /*Assert.NotNull(result1);
        Assert.IsType<BusinessException>(result1);
        Assert.Equal("409.006", result1.Code);
        Assert.Equal("No existe el registro.", result1.Message);*/
        
    }

}