using Application.Common.Utilities;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Calzolari.Grpc.AspNetCore.Validation;
using GrpcApiService.ServiceName.Configuration;
using GrpcApiService.ServiceName.Services;
using Infraestructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SC.Configuration.Provider.Mongo;
using Serilog;
using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;
IConfiguration configuration = builder.Configuration;
#region Host Configuration
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    IConfigurationRoot configurationRoot = (IConfigurationRoot)config
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment.ApplicationName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
    AddkeyValult(config, configurationRoot);

}).UseSerilog((hostBuilder, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostBuilder.Configuration);
    loggerConfiguration.WriteTo.Console();

});
#endregion Host Configuration

string mongoConnection = configuration.GetValue<string>(configuration.GetSection("MongoConfigurationProvider:ConnectionString").Value);
builder.Configuration.AddMongoProvider(nameof(MongoConfigurationProvider), mongoConnection);
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnection").Value);
BusinessSettings settings = builder.Configuration.GetSection(nameof(BusinessSettings)).Get<BusinessSettings>();
builder.Services.Configure<BusinessSettings>(builder.Configuration.GetRequiredSection(nameof(BusinessSettings)));
string country = settings.DefaultCountry;

#region Service Configuration
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, Convert.ToInt32(settings.GRPCPort), listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services
    .RegisterAutoMapper()
    .AddConfigureDatabaseSQL(configuration)
    .RegisterAutoMapper()
    .RegisterServices(configuration, ServiceBusConnectionSecret)
    .AddGrpConfig();

builder.Services
    .AddHealthChecks();


#endregion Service Configuration

builder.Services.AddGrpcValidations();

builder.Services.AddValidator();
builder.Services.AddHealth();

WebApplication app = builder.Build();

#region Enable middle-ware to serve generated Swagger as a JSON endpoint.

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Configure the HTTP request pipeline.
    endpoints.MapGrpcService<NotesService>();
    app.MapGrpcService<NoteListsService>();
    app.MapGrpcService<SettingsService>();
    endpoints.MapGet("/",
        async context =>
        {
            await context.Response.WriteAsync(
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        });
});

app.MapGrpcHealthChecksService();
app.Run();

#endregion

#region ConfigurationKeyVault
void AddkeyValult(IConfigurationBuilder config, IConfigurationRoot configurationRoot)
{
    ClientSecretCredential clientSecretCredential = new ClientSecretCredential(builder.Configuration["AzureKeyVaultConfig:TenantId"], builder.Configuration["AzureKeyVaultConfig:AppId"], builder.Configuration["AzureKeyVaultConfig:AppSecret"]);
    SecretClient client = new SecretClient(new Uri(builder.Configuration["AzureKeyVaultConfig:KeyVault"]), clientSecretCredential);
    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
}
#endregion ConfigurationKeyVault