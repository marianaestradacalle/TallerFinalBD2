using Application.DTOs.Notes;
using Application.Services.Simple;
using AutoMapper;
using Common.Helpers.Exceptions;
using Core.Entities;
using Core.Enumerations;
using Dobles.Tests.Application.DTOs.Tests.Notes;
using Dobles.Tests.Dummy.Generic;
using Dobles.Tests.Dummy.InfrastructureServices;
using Dobles.Tests.Fake.Generic;
using Dobles.Tests.Fake.InfrastructureServices;
using Dobles.Tests.Mocks.Generic;
using Dobles.Tests.Stubs.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Unit.Tests.Application.Tests.Services.Simple;
public class NotesServiceTests
{    
    private DummyNotificationServiceEventAdapter dummyNotificationServiceEventAdapter = new DummyNotificationServiceEventAdapter();
    private DummyUnitWork dummyUnitWork = new DummyUnitWork();
    private FakeGenericRepositoryServiceNotes<Notes> fakeGenericRepositoryService = new FakeGenericRepositoryServiceNotes<Notes>();
    private MockConfiguration<NotesService> mockConfiguration = new MockConfiguration<NotesService>();
    public NotesService CreateNotesService;
    private StubSettings stubSettings = new StubSettings();
    private FakeMapper fakeMapper = new FakeMapper();


    public NotesServiceTests() 
    {   
        CreateNotesService= new NotesService(fakeGenericRepositoryService,
                                    (ILogger<NotesService>)mockConfiguration.MockLogger(),
                                    (IMapper)fakeMapper.GetFakeMapper(),
                                    dummyNotificationServiceEventAdapter,
                                    stubSettings.StubSetting(),
                                    dummyUnitWork);
    }    


    [Fact]
    public async Task CreateNote_ValidRegister_ReturnNote()
    {
        // Arrange                 
        NoteInput noteInput = NoteInputStatic.GetNoteInput();

        // Act
        var result = await CreateNotesService.CreateNote(noteInput);       
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoteOutput>();
        result.Title.Should().Be("test");
        result.State.Should().Be((int)NoteStates.NOTCHECKED);
        result.Id.Should().NotBeEquivalentTo(Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        
        Assert.NotNull(result);
        Assert.IsType<NoteOutput>(result);
        Assert.NotEqual(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal(result.State, (int)NoteStates.NOTCHECKED);
    }
    [Fact]
    public async Task GetNote_GetValidNoteById_ReturnNote()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();

        // Act
        var result = await CreateNotesService.GetNote(_id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoteOutput>();
        result.Title.Should().Be("test");
        result.Id.Should().Be(_id);

        Assert.NotNull(result);
        Assert.IsType<NoteOutput>(result);
        Assert.Equal(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal("test", result.Title);
        
    }

    [Fact]
    public async Task UpdateNote_ValidUpdateNoteById_ReturnTrue()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        ModifiedNoteInput data = ModifiedNoteInputStatic.GetModifiedNoteInput();

        // Act
        var result = await CreateNotesService.UpdateNote(_id, data);

        // Assert
        result.Should().BeTrue();
        
        Assert.True(result);

    }

    [Fact]
    public async Task DeleteNote_ValidDeletedNoteById_ReturnTrue()
    {
        // Arrange
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        // Act
        var result = await CreateNotesService.DeleteNote(_id);

        // Assert
        result.Should().BeTrue();

        Assert.True(result);
    }

    [Fact]
    public async Task GetNote_GetInvalidNoteById_ReturnException()
    {
        // Arrange
        string _id = "";

        // Act
        var result1 = await Assert.ThrowsAnyAsync<BusinessException>(async () => { await CreateNotesService.GetNote(_id); });
        Func<Task> result = async () => { await CreateNotesService.GetNote(_id); };
        
        // Assert        
        await result.Should().ThrowAsync<BusinessException>();

        Assert.NotNull(result1);
        Assert.IsType<BusinessException>(result1);
        Assert.Equal("409.006", result1.Code);
        Assert.Equal("No existe el registro.", result1.Message);        
    }
    
}