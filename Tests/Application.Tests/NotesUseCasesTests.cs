using Application.Common.Tests.Helpers.Builders;
using Application.Common.Utilities;
using Application.DTOs;
using Application.DTOs.Notes;
using Application.Interfaces.Infraestructure;
using Application.Services.Simple;
using AutoMapper;
using Common.Helpers.Exceptions;
using Common.Tests.Utilities.Builders;
using Core.Entities;
using Core.Enumerations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Services.MSQLServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests;
public class NotesUseCasesTests
{
    private MockRepository _mockRepository;

    private readonly Mock<IGenericRepositoryService<Notes>> _mockNotesRepository;
    private readonly Mock<ILogger<NotesService>> mockLogger;
    private readonly IMapper _mockMapper;
    private readonly Mock<IUnitWork> _mockUnitWork;
    private readonly Mock<ContextSQLServer> mockSqlDb = new Mock<ContextSQLServer>();
    private readonly Mock<INotificationServiceBusService> mockNotificationEventAdapter = new Mock<INotificationServiceBusService>();
    private readonly Mock<IOptionsMonitor<BusinessSettings>> _appSettings = new();


    public NotesUseCasesTests() 
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _mockUnitWork = new Mock<IUnitWork>();

        _mockNotesRepository = _mockRepository.Create<IGenericRepositoryService<Notes>>();
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

    private NotesService CreateNotesUseCases()
    {
        return new NotesService(_mockNotesRepository.Object,
                                mockLogger.Object,
                                _mockMapper,
                                mockNotificationEventAdapter.Object,
                                _appSettings.Object,
                                _mockUnitWork.Object);
    }

    [Fact]
    public async Task NotesUseCases_CreateNote_RegistraElObjetoNotesEnviadoComoParametroEnLaBaseDeDatos_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCase = CreateNotesUseCases();        
        Notes notesCreated = new NotesBuilder().Notes();
        NoteInput noteInput = new NoteInput() { Title = notesCreated.Title };

        
        _mockNotesRepository.Setup(_ => _.AddAsync(It.IsAny<Notes>()))
               .Verifiable();
        // Act
        var result = await notesUseCase.CreateNote(noteInput);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NoteOutput>(result);
        Assert.NotEqual(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal(result.Title, "test");
        Assert.Equal(result.State, (int)NoteStates.NOTCHECKED);
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task NotesUseCases_GetNote_RetornaElObjetoNotesAsociadoAlIdEnviadoComoParametro_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCases = CreateNotesUseCases();
        Notes note = new NotesBuilder().Notes();

        _mockNotesRepository.Setup(_ => _.FindByIdAsync(It.IsAny<object>()))
            .ReturnsAsync(note)
            .Verifiable();

        // Act
        var result = await notesUseCases.GetNote(note.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NoteOutput>(result);
        Assert.Equal(result.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal(result.Title, "test");
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task NotesUseCases_UpdateNote_ElRegistroDeLaNotesEnviadoComoParametroEnLaBaseDeDatos_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCases = CreateNotesUseCases();
        Notes note = new NotesBuilder().Notes();
        ModifiedNoteInput data = new ModifiedNoteInput() { Title=note.Title, State=(int)note.State};

        _mockNotesRepository.Setup(_ => _.FindByIdAsync(It.IsAny<object>()))
            .ReturnsAsync(note)
            .Verifiable();
        _mockNotesRepository.Setup(_ => _.Update(It.IsAny<Notes>()))
            .Verifiable();

        // Act
        await notesUseCases.UpdateNote(note.Id, data);

        // Assert
        Assert.Equal(note.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
        Assert.Equal(note.Title, "test");
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task NotesUseCases_DeleteAsync_EliminaElRegistroDelNoteAsociadoAlIdEnviadoComoParametroDeLaBaseDeDatos_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCases = CreateNotesUseCases();
        Notes note = new NotesBuilder().Notes();

        _mockNotesRepository.Setup(_ => _.FindByIdAsync(It.IsAny<object>()))
            .ReturnsAsync(note)
            .Verifiable();

        _mockNotesRepository.Setup(_ => _.Delete(It.IsAny<Notes>()))
            .Verifiable();

        // Act
        await notesUseCases.DeleteNote(note.Id);

        // Assert
        Assert.Equal(note.Id, Guid.Parse("b253f947-fd75-4cf2-863f-37cbe04d2f2f").ToString());
    }

    [Fact]
    public async Task NotesUseCases_GetByIdAsync_RetornaBusinessExceptionRecordNotFoundCuandoNoHayNingunRegistroEnLaBaseDeDatosAsociadoAlIdEnviadoComoParametro_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCases = CreateNotesUseCases();
        Notes note = new NotesBuilder().Notes();
        Notes? notes = null;

        _mockNotesRepository.Setup(_ => _.FindByIdAsync(It.IsAny<object>()))
          .ReturnsAsync(notes)
          .Verifiable();

        // Act
        var result = await Assert.ThrowsAnyAsync<BusinessException>(async () => { await notesUseCases.GetNote(note.Id); });

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BusinessException>(result);
        Assert.Equal(result.Code, "409.006");
        Assert.Equal(result.Message, "No existe el registro.");
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task NotesUseCases_DeleteAsync_RetornaBusinessExceptionRecordNotFoundCuandoNoHayNingunRegistroEnLaBaseDeDatosAsociadoAlIdEnviadoComoParametro_Cuando_SeEjecutaElMetodo()
    {
        // Arrange
        var notesUseCases = CreateNotesUseCases();
        Notes note = new NotesBuilder().Notes();
        Notes? notes = null;

        _mockNotesRepository.Setup(_ => _.FindByIdAsync(It.IsAny<object>()))
         .ReturnsAsync(notes)
         .Verifiable();

        // Act
        var result = await Assert.ThrowsAnyAsync<BusinessException>(async () => { await notesUseCases.GetNote(note.Id); });

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BusinessException>(result);
        Assert.Equal(result.Code, "409.006");
        Assert.Equal(result.Message, "No existe el registro.");
        _mockRepository.VerifyAll();
    }

}
