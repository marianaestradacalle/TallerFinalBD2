using Application.Common.Utilities;
using Application.DTOs;
using Application.DTOs.Notes;
using Application.Interfaces.Services;
using Application.Services.Simple;
using Application.Tests.Common.Utilities;
using Application.Tests.DTOs.Notes;
using AutoMapper;
using Common.Helpers.Exceptions;
using Core.Entities;
using Core.Enumerations;
using Dobles.Tests.Fake;
using Dobles.Tests.Stubs;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Services.Simple;
public class NotesServiceTests
{
    private MockRepository _mockRepository;
    private readonly Mock<ILogger<NotesService>> mockLogger;
    private readonly Mock<IOptionsMonitor<BusinessSettings>> _appSettings = new();
    private readonly IMapper _mockMapper;
    public NotesServiceTests() 
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        mockLogger = _mockRepository.Create<ILogger<NotesService>>();
        _appSettings.Setup(settings => settings.CurrentValue).Returns(new AppSettingsBuilder()
            .WithServiceExceptionByDefault(new ServiceExceptionBuilder().Build())
            .WithServiceExceptions(new List<ServiceException>
            {
                new ServiceExceptionBuilder().WithId("RecordNotFound").WithCode("409.006").WithMessage("No existe el registro.").Build(),
            })
            .Build());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mockMapper = config.CreateMapper();


    }

    private StubNotificationServiceEventAdapter stubNotificationServiceEventAdapter = new StubNotificationServiceEventAdapter();
    private StubUnitWork stubUnitWork = new StubUnitWork();    
    private FakeGenericRepositoryServiceNotes<Notes> fakeGenericRepositoryService = new FakeGenericRepositoryServiceNotes<Notes>();
    private NotesService CreateNotesService()
    {
        return new NotesService(fakeGenericRepositoryService,
                                mockLogger.Object,
                                _mockMapper,
                                stubNotificationServiceEventAdapter,
                                _appSettings.Object,
                                stubUnitWork);

    }

    [Fact]
    public async Task CreateNote_ValidRegister_ReturnNote()
    {
        // Arrange
        var notesUseCase = CreateNotesService();           
        NoteInput noteInput = NoteInputStatic.GetNoteInput();

        // Act
        var result = await notesUseCase.CreateNote(noteInput);       
        
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

    /*[Fact]
    public async Task CreateNote_InvalidRegister_ReturnException()
    {
        // Arrange
        var notesUseCase = CreateNotesService();        
        NoteInput noteInput = new NoteInput() { Title = "TEST" };

        // Act

        Action result = () => notesUseCase.CreateNote(noteInput);

        // Assert
        result.Should().Throw<NotImplementedException>();

        await Assert.ThrowsAsync<BusinessException>(() => notesUseCase.CreateNote(noteInput));

        
    }*/

    [Fact]
    public async Task GetNote_GetValidNoteById_ReturnNote()
    {
        // Arrange
        var notesUseCases = CreateNotesService();
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();

        // Act
        var result = await notesUseCases.GetNote(_id);

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
    public async Task UpdateNote_ValidUpdateNoteById_WithOutReturn()
    {
        // Arrange
        var notesUseCases = CreateNotesService();
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        ModifiedNoteInput data = ModifiedNoteInputStatic.GetModifiedNoteInput();

        // Act
        await notesUseCases.UpdateNote(_id, data);
        Action result = () => notesUseCases.UpdateNote(_id, data);

        // Assert
        //result.Should().Th>();

        // Assert
        //result.Should().NotBeNull();
        //result.Should().BeOfType<NoteOutput>();
        //result.Title.Should().Be("test");
        //result.State.Should().Be((int)NoteStates.NOTCHECKED);
        //result.Id.Should().NotBeEquivalentTo(Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());

        //Assert.Equal(note.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        //Assert.Equal(note.Title, "test");

    }

    [Fact]
    public async Task DeleteNote_ValidDeletedNoteById_ReturnTrue()
    {
        // Arrange
        var notesUseCases = CreateNotesService();
        string _id = Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString();
        // Act
        await notesUseCases.DeleteNote(_id);

        // Assert
        //Assert.Equal(note.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
    }

    [Fact]
    public async Task GetNote_GetInvalidNoteById_ReturnException()
    {
        // Arrange
        var notesUseCases = CreateNotesService();
        string _id = "";

        // Act
        var result1 = await Assert.ThrowsAnyAsync<BusinessException>(async () => { await notesUseCases.GetNote(_id); });
        //var result = await this.Awaiting(_ => notesUseCases.GetNote(_id)).Should().ThrowAsync<BusinessException>();
        Func<Task> result = async () => { await notesUseCases.GetNote(_id); };
        
        // Assert        
        await result.Should().ThrowAsync<BusinessException>();

        //result.Should().Which("No existe el registro.");



        // result.Should().ThrowBusinessException>();


        Assert.NotNull(result1);
        Assert.IsType<BusinessException>(result1);
        Assert.Equal("409.006", result1.Code);
        Assert.Equal("No existe el registro.", result1.Message);
        
    }

}