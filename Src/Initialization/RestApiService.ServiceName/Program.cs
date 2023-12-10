using Infrastructure;
using RestApi.Middlewares;
using RestApiService.ServiceName.Configuration;
using SC.Configuration.Provider.Mongo;
using Serilog;


// Configuraciones servicios
var builder = WebApplication.CreateBuilder(args);
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
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnection").Value);
string MongoConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:MongoConnection").Value);

builder.Configuration.AddJsonProvider();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongoDataBase(MongoConnectionSecret, builder.Configuration.GetSection("AppSettings:Database").Value);
builder.Services.RegisterServices(configuration, ServiceBusConnectionSecret);
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
    var settings = new MongoAppsettingsConfiguration();
    configurationRoot.GetSection("MongoConfigurationProvider").Bind(settings);
    config.AddMongoConfiguration(options =>
    {
        options.CollectionName = configuration["MongoConfigurationProvider:CollectionName"];
        options.ConnectionString = configuration.GetValue<string>(builder.Configuration.GetSection("MongoConfigurationProvider:ConnectionString").Value); //settings.ConnectionString];
        options.DatabaseName = configuration["MongoConfigurationProvider:DatabaseName"];
        options.ReloadOnChange = true;
    });
}
#endregion ConfigurationMongoProvider
