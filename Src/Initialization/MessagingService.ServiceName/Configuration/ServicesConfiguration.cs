using Application.Common.Utilities;
using Application.DTOs;
using Application.DTOs.Notes;
using Application.Interfaces.Infraestructure;
using Application.Interfaces.Services;
using Application.Services.Simple;
using AutoMapper.Data;
using Core.Entities;
using Infraestructure;
using MessagingService.ServiceName.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Services.MSQLServer;

namespace MessagingService.ServiceName.Configuration;
public static class ServicesConfiguration
{
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(cfg =>
        {
            cfg.AddDataReaderMapping();
        }, typeof(MappingProfile));

    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, ContextSQLServer>();
        services.AddScoped<IGenericRepositoryService<Notes>, GenericRepositoryService<Notes>>();
        services.AddScoped<IUnitWork, UnitWork>();
        services.AddScoped<INotesService, NotesService>();

        services.AddAdaptersAzServiceBus();
        services.AddMvc();
        services.AddInfrastructure();
        services.AddSubscriptions();
        services.AddMongoProvidersConfiguration(configuration);
        return services;
    }

    public static IServiceCollection AddAsyncGateways(this IServiceCollection services, string serviceBusConnection)
    {
        services.AddAsyncGateway<dynamic>(serviceBusConnection);
        services.AddAsyncGateway<NoteInput>(serviceBusConnection);
        services.AddAsyncGateway<NoteOutput>(serviceBusConnection);
        services.AddAsyncGateway<Notes>(serviceBusConnection);
        return services;
    }
    public static IServiceCollection AddSubscriptions(this IServiceCollection services)
    {
        services.AddScoped<INotesSubscription, NotesSubscription>();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHostedService<TransferSubscriptionsHostedService>();

        return services;
    }
    private static IServiceCollection AddMongoProvidersConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.LoadSettings<BusinessSettings>(configuration);
        return services;
    }
}
