using Application.Interfaces.Infrastructure;
using Infrastructure.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.MSQLServer;

namespace Infrastructure;
public static class InfrastructureDependencyInjection 
{
    public static IServiceCollection AddConfigureDatabaseSQL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContextSQLServer>(options =>
           options.UseSqlServer(configuration["DataBase:ConnectionString"]));

        return services;
    }
    public static IServiceCollection AddMongoDataBase(this IServiceCollection services, string mongoConnectionString, string dataBaseName)
    {
        services.AddScoped<IEventoRepository, EventoAdapter>();
        services.AddSingleton(provider => new Context(mongoConnectionString, $"{dataBaseName}"));
        return services;
    }
}
