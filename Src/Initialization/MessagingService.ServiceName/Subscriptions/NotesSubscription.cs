using Application.Common.Utilities;
using Application.DTOs.Notes;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using org.reactivecommons.api;
using org.reactivecommons.api.domain;
using System.Reflection;

namespace MessagingService.ServiceName.Subscriptions;
public class NotesSubscription : SubscriptionBase<NotesSubscription>, INotesSubscription
{
    private readonly IDirectAsyncGateway<NoteInput> _directAsyncGateway;
    private readonly INotesService _notesUseCases;
    private readonly IOptionsMonitor<BusinessSettings> _configuradorAppSettings;

    public NotesSubscription(IOptionsMonitor<BusinessSettings> configuradorAppSettings,
            IDirectAsyncGateway<NoteInput> directAsyncGateway,
            INotesService notesUseCases)
           : base()
    {
        _directAsyncGateway = directAsyncGateway;
        _notesUseCases = notesUseCases;
        _configuradorAppSettings = configuradorAppSettings;
    }

    public async Task SubscribeAsync()
    {
        await SubscribeOnCommandAsync(_directAsyncGateway, _configuradorAppSettings.CurrentValue.Requestqueue,
            CreateNoteSubscription,
            MethodBase.GetCurrentMethod());

        await SubscribeOnEventAsync(_directAsyncGateway, _configuradorAppSettings.CurrentValue.Requesttopic,
               "msv-ddd", CreateNoteSubscriptionEvent, MethodBase.GetCurrentMethod(), 50);
    }

    private async Task CreateNoteSubscription(Command<NoteInput> command) =>
           await HandleCommandAsync(async (noteInput) =>
           {
               await _notesUseCases.CreateNote(noteInput);
           },
           MethodBase.GetCurrentMethod(),
           command.data.Title,
           command,
           notifyBusinessException: true);

    private async Task CreateNoteSubscriptionEvent(DomainEvent<NoteInput> eventResponse) =>
    await HandleEventAsync(async (noteInput) =>
    {
        await _notesUseCases.CreateNote(noteInput);
    },
    MethodBase.GetCurrentMethod(),
    $"{eventResponse.data.Title}",
    eventResponse,
    notifyBusinessException: true);
}
