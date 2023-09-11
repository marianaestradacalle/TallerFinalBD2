using Application;
using Application.Common.Utilities;
using Application.DTOs;
using Application.Interfaces.Infrastructure;
using Calzolari.Grpc.AspNetCore.Validation;
using Core.Entities;
using GrpcApi.Exceptions;
using GrpcApiService.ServiceName.Services;
using GrpcApiService.ServiceName.Validations;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Services.MSQLServer;

namespace GrpcApiService.ServiceName.Configuration;
public static class ServicesConfiguration
{
    public static IServiceCollection RegisterCors(this IServiceCollection services, string policyName)
         => services.AddCors(o =>
         {
             o.AddPolicy(policyName, builder =>
             {
                 builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
             });
         });
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddAutoMapper(typeof(GRPCProfile));

        return services;
    }
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration, string servicesBusConnection)
    {
        #region Adaptadores
        services.AddScoped<DbContext, ContextSQLServer>();
        services.AddScoped<IGenericRepositoryAdapter<Notes>, GenericRepositoryService<Notes>>();
        services.AddScoped<IGenericRepositoryAdapter<NoteLists>, GenericRepositoryService<NoteLists>>();
        services.AddScoped<IUnitWork, UnitWork>();

        services.AddAsyncGateway<dynamic>(servicesBusConnection);
        services.AddAdaptersAzServiceBus();
        #endregion Adaptadores
        #region UseCases
        services.AddUseCases();
        #endregion UseCases
        services.AddMongoProvidersConfiguration(configuration);
        return services;
    }

    private static IServiceCollection AddMongoProvidersConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.LoadSettings<BusinessSettings>(configuration);
        return services;
    }

    public static IServiceCollection AddGrpConfig(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.EnableMessageValidation();
            options.Interceptors.Add<ExceptiongRPCInterceptor>(); // Register custom ExceptionInterceptor interceptor
        })
        .AddServiceOptions<NoteListsService>(options => {
            options.EnableMessageValidation();
            options.Interceptors.Add<ExceptiongRPCInterceptor>(); // Register custom ExceptionInterceptor interceptor
        })
        .AddServiceOptions<NotesService>(options => {
            options.EnableMessageValidation();
            options.Interceptors.Add<ExceptiongRPCInterceptor>(); // Register custom ExceptionInterceptor interceptor
        })
        .AddServiceOptions<SettingsService>(options => {
            options.EnableMessageValidation();
            options.Interceptors.Add<ExceptiongRPCInterceptor>(); // Register custom ExceptionInterceptor interceptor
        });

        return services;
    }

    public static IServiceCollection AddGrpcValidations(this IServiceCollection services)
    {
        services.AddGrpcValidation();

        return services;
    }

    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddValidator<NoteInputValidation>();

        return services;
    }

    public static IServiceCollection AddHealth(this IServiceCollection services)
    {

        services.AddGrpcHealthChecks()
                .AddCheck("health", () => HealthCheckResult.Healthy());

        return services;
    }

}
