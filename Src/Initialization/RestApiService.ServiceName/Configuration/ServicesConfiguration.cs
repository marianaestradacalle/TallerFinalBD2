using Application;
using Application.Common.Utilities;
using Application.Interfaces.Infrastructure;
using Infrastructure;
using Infrastructure.Services.MongoDB;
using Infrastructure.Services.MSQLServer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestApi.Filters;
using RestApi.Health;
using Services.MSQLServer;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestApiService.ServiceName.Configuration;
public static class ServicesConfiguration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAutoMapper();
        services.AddConfigureDatabaseSQL(configuration);

        #region Adaptadores
        services.AddScoped<DbContext, ContextSQLServer>();
        services.AddScoped<IEventoRepository,EventoAdapter>();
        services.AddScoped<IGenericRepositoryAdapter<IAsistenteRepository>, GenericRepositoryService<IAsistenteRepository>>();
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
            options.Filters.Add<SuccessFilter>();
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
