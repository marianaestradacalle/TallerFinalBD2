using Application.Interfaces.Infrastructure;
using Infrastructure.Services.AzServiceBus;
using Infrastructure.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using org.reactivecommons.api;
using org.reactivecommons.api.impl;
using Services.MSQLServer;

namespace Infrastructure;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddAdaptersAzServiceBus(this IServiceCollection services)
    {
        services.AddScoped<INotificationServiceEventAdapter, NotificationServiceBusService>();
        services.AddScoped(typeof(IGenericServiceEventAdapter<>),typeof(Services.AzServiceBus.GenericServiceBusService<>));
        return services;
    }

    public static IServiceCollection AddAsyncGateway<TEntity>(this IServiceCollection services, string serviceBusConnection)
    {
        services.AddSingleton<IDirectAsyncGateway<TEntity>>(new DirectAsyncGatewayServiceBus<TEntity>(serviceBusConnection));

        return services;
    }

    public static IServiceCollection AddConfigureDatabaseSQL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContextSQLServer>(options =>
           options.UseSqlServer(configuration["DataBase:ConnectionString"]));

        return services;
    }
    public static IServiceCollection AddMongoDataBase(this IServiceCollection services, string mongoConnectionString, string dataBaseName)
    {
        services.AddScoped<INotasRepository, NotasAdapter>();
        services.AddSingleton<IContext>(provider => new Context(mongoConnectionString, $"{dataBaseName}"));
        return services;
    }
}
