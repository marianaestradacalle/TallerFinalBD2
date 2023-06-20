using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infraestructure;
using RpcApi.Middlewares;
using RpcApiService.ServiceName.Configuration;
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
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnection").Value);
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

builder.Services.UseRpcApiFilters();

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
    if (environment.EnvironmentName.Equals("Local"))
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