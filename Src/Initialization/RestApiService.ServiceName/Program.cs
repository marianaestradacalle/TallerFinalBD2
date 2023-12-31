using Infrastructure;
using RestApi.Middlewares;
using RestApiService.ServiceName.Configuration;
using SC.Configuration.Provider.Mongo;
using Serilog;


// Configuraciones servicios
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;
IConfiguration configuration = builder.Configuration;

#region ProgramConfiguration
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    IConfigurationRoot configurationRoot = (IConfigurationRoot)config
        .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment.ApplicationName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
        AddMongoprovider(config, configurationRoot);

}).UseSerilog((hostBuilder, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostBuilder.Configuration);
    loggerConfiguration.WriteTo.Console();
});

#endregion ProgramConfiguration

builder.Services.AddControllers();
builder.Services.UseRestApiFilters();
string MongoConnectionSecret = builder.Configuration["MongoConfigurationProvider:ConnectionString"];

builder.Configuration.AddJsonProvider();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongoDataBase(MongoConnectionSecret, builder.Configuration["MongoConfigurationProvider:DatabaseName"]);
builder.Services.RegisterServices(configuration);
builder.Services.AddHealthChecks();
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration.WriteTo.Console();
});

// Configuraciones de aplicación
WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);
app.UseHttpsRedirection();
app.ServiceHealthChecks(builder.Configuration["AppSettings:HealthChecksEndPoint"], builder.Configuration["AppSettings:DomainName"]);
app.UseAuthorization();
app.MapControllers();
app.Run();

#region ConfigurationMongoProvider
void AddMongoprovider(IConfigurationBuilder config, IConfigurationRoot configurationRoot)
{
    MongoAppsettingsConfiguration? settings = new();
    configurationRoot.GetSection("MongoConfigurationProvider").Bind(settings);
    config.AddMongoConfiguration(options =>
    {
        options.CollectionName = configuration["MongoConfigurationProvider:CollectionName"];
        options.ConnectionString = configuration["MongoConfigurationProvider:ConnectionString"]; //settings.ConnectionString];
        options.DatabaseName = configuration["MongoConfigurationProvider:DatabaseName"];
        options.ReloadOnChange = true;
    });
}
#endregion ConfigurationMongoProvider
