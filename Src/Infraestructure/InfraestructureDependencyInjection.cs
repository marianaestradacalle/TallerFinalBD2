using Application.Interfaces.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using org.reactivecommons.api;
using org.reactivecommons.api.impl;
using Services.AzServiceBus;
using Services.MSQLServer;

namespace Infraestructure;
public static class InfraestructureDependencyInjection
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
}
