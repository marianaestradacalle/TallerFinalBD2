using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Infraestructure;
using RestApi.Middlewares;
using RestApi.Validations;
using RestApiService.ServiceName.Configuration;
using RestApiService.ServiceName.Validations;
using SC.Configuration.Provider.Mongo;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


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
    AddkeyValult(config, configurationRoot, environment);
    AddMongoprovider(config, configurationRoot);

}).UseSerilog((hostBuilder, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostBuilder.Configuration);
    loggerConfiguration.WriteTo.Console();

});

#endregion ProgramConfiguration

builder.Services.AddControllers();
builder.Services.UseRestApiFilters();
builder.Services.AddValidators();
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnection").Value);
builder.Configuration.AddJsonProvider();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterAutoMapper();
builder.Services.AddConfigureDatabaseSQL(configuration);
builder.Services.RegisterAutoMapper();
builder.Services.RegisterServices(configuration, ServiceBusConnectionSecret);
builder.Services.AddHealthChecks();
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration.WriteTo.Console();
});

// Configuraciones de aplicación
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);
app.UseHttpsRedirection();
app.ServiceHealthChecks(builder.Configuration["AppSettings:HealthChecksEndPoint"], builder.Configuration["AppSettings:DomainName"]);
app.UseAuthorization();
app.MapControllers();
app.Run();

#region ConfigurationKeyVault
void AddkeyValult(IConfigurationBuilder config, IConfigurationRoot configurationRoot, IWebHostEnvironment environment)
{
    if ((builder.Configuration["AppSettings:Environment"]) == "Local")
    {
        ClientSecretCredential clientSecretCredential = new ClientSecretCredential(builder.Configuration["AzureKeyVaultConfig:TenantId"], builder.Configuration["AzureKeyVaultConfig:AppId"], builder.Configuration["AzureKeyVaultConfig:AppSecret"]);
        SecretClient client = new SecretClient(new Uri(builder.Configuration["AzureKeyVaultConfig:KeyVault"]), clientSecretCredential);
        config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
    }
    else
    {
        config.AddAzureKeyVault(new Uri(configurationRoot["AzureKeyVaultConfig:KeyVault"]), new DefaultAzureCredential());
    }
   
}
#endregion ConfigurationKeyVault

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
