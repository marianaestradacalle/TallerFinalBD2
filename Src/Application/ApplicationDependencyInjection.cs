using Application.Interfaces.Services;
using Application.Services.Compound;
using Application.Services.Simple;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<INoteListsService, NoteListsService>();
        services.AddScoped<INotesService, NotesService>();
        services.AddScoped<IClearAllService, ClearAllService>();
        services.AddScoped<INoteCleaningService, NoteCleaningService>();
        return services;
    }
}
