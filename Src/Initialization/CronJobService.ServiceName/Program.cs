using Application.Common.Utilities;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CronJobService.ServiceName;
using CronJobService.ServiceName.Configuration;
using Hangfire;
using Infraestructure;
using SC.Configuration.Provider.Mongo;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;
IConfiguration configuration = builder.Configuration;

#region ProgramConfiguration
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    IConfigurationRoot configurationRoot = (IConfigurationRoot)config
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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

#region ConfigureServices
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnectionSecret").Value);
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterAutoMapper();
builder.Services.AddConfigureDatabaseSQL(configuration);
builder.Services.RegisterAutoMapper();
builder.Services.ConfigureServices(builder.Configuration, builder.Configuration["Secrets:ServiceBusConnectionSecret"], builder.Configuration["AppSettings:HangFireConnectionString"]);
BusinessSettings settings = builder.Configuration.GetSection(nameof(BusinessSettings)).Get<BusinessSettings>();
builder.Services.Configure<BusinessSettings>(builder.Configuration.GetRequiredSection(nameof(BusinessSettings)));

#endregion ConfigureServices


WebApplication app = builder.Build();

#region ConfigureMiddleware
app.UseHangfireDashboard();
var creatorNotesJob = app.Services.GetRequiredService<IEnumerable<ICreatorNotesJob>>();
// register job with container  
IServiceProvider ApplicationBuilder = app.Services.GetRequiredService<IServiceProvider>();
ApplicationBuilder.AddJobs(settings, creatorNotesJob: creatorNotesJob);
app.UseStaticFiles(); // For the wwwroot folder
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("");
        });
        // endpoints.MapHangfireDashboard();
    });
}
else
{
    app.UseHsts();
}

app.Run();
#endregion ConfigureMiddleware

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