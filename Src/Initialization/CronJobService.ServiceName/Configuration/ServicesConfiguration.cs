using Application.Common.Utilities;
using Application.DTOs;
using Application.Interfaces.Infraestructure;
using Application.Interfaces.Services;
using Application.Services.Simple;
using AutoMapper;
using Coravel;
using Core.Entities;
using Hangfire;
using Hangfire.SqlServer;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.MSQLServer;

namespace CronJobService.ServiceName.Configuration;
public static class ServicesConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services, string serviceBusConn)
    {
        #region Adapters
        services.AddScoped<DbContext, ContextSQLServer>();
        services.AddScoped<IGenericRepositoryAdapter<Notes>, GenericRepositoryService<Notes>>();
        services.AddScoped<IUnitWork, UnitWork>();
        services.AddAsyncGateway<dynamic>(serviceBusConn);
        services.AddAdaptersAzServiceBus();


        #endregion

        #region Jobs
        services.AddTransient<ICreatorNotesJob, CreatorNotesJob>();
        #endregion

        #region UseCase
        services.AddTransient<INotesUseCase>(provider => new NotesService(
            services.BuildServiceProvider().GetService<IGenericRepositoryAdapter<Notes>>(),
            services.BuildServiceProvider().GetService<ILogger<NotesService>>(),
            services.BuildServiceProvider().GetService<IMapper>(),
            services.BuildServiceProvider().GetService<INotificationServiceEventAdapter>(),
            services.BuildServiceProvider().GetService<IOptionsMonitor<BusinessSettings>>(),
            services.BuildServiceProvider().GetService<IUnitWork>()));
        #endregion UseCase

        services.AddScheduler();
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, string ServiceBusConnectionSecret, string HangFireConnectionString)
    {
        string servicesBusConnection = configuration[ServiceBusConnectionSecret];
        services.AddServices(servicesBusConnection);
        services.AddMongoProvidersConfiguration(configuration);
        services.AddConfigHangfire(HangFireConnectionString);

        return services
                .AddHangfireServer()
                .AddScheduler();


    }
    public static IServiceCollection AddConfigHangfire(this IServiceCollection services, string HangFireConnectionString)
    {
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(HangFireConnectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                SchemaName = "Test"
            }));

        return services;
    }
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
    private static IServiceCollection AddMongoProvidersConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.LoadSettings<BusinessSettings>(configuration);
        return services;
    }

}
