using Application;
using Application.Common.Utilities;
using Application.DTOs;
using Application.Interfaces.Infraestructure;
using Core.Entities;
using FluentValidation;
using Infraestructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestApi.Filters;
using RestApi.Health;
using RestApi.Validations;
using RestApiService.ServiceName.Validations;
using Services.MSQLServer;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestApiService.ServiceName.Configuration;
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

    public static IConfigurationBuilder AddJsonProvider(this IConfigurationBuilder configuration)
    {
        configuration
                    .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);

        return configuration;
    }

    public static IServiceCollection UseRestApiFilters(this IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<NoteListInputValidation>();
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableBuiltInModelValidation = false;

            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        return services;
    }

    public static IApplicationBuilder ServiceHealthChecks(this IApplicationBuilder app, string endpoint, string serviceName)
    {
        return app.UseHealthChecks(endpoint, new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                string result = JsonSerializer.Serialize(
                new HealthResult
                {
                    ServiceName = serviceName,
                    Status = report.Status.ToString(),
                    Duration = report.TotalDuration,
                    Checks = report.Entries.Select(e => new HealthInfo
                    {
                        Name = e.Key,
                        Description = e.Value.Description,
                        Duration = e.Value.Duration,
                        Status = Enum.GetName(typeof(HealthStatus),
                                                e.Value.Status),
                        Error = e.Value.Exception?.Message
                    }).ToList()
                },
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = false
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            }
        });
    }

    private static IServiceCollection AddMongoProvidersConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.LoadSettings<BusinessSettings>(configuration);
        return services;
    }

}
