using Infrastructure;
using MessagingService.ServiceName.Configuration;
using SC.Configuration.Provider.Mongo;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;
IConfiguration configuration = builder.Configuration;
#region Host Configuration

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonProvider(environment);

builder.Configuration.AddKeyVaultProvider(environment);

builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

#endregion Host Configuration

#region Service Configuration
string ServiceBusConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:ServiceBusConnection").Value);
string MongoConnectionSecret = builder.Configuration.GetValue<string>(builder.Configuration.GetSection("Secrets:MongoConnection").Value);
builder.Services
    .AddMongoDataBase(MongoConnectionSecret, builder.Configuration.GetSection("AppSettings:Database").Value)
    .RegisterAutoMapper()
    .AddConfigureDatabaseSQL(configuration)
    .AddAsyncGateways(ServiceBusConnectionSecret)
    .RegisterAutoMapper()
    .RegisterServices(configuration);

builder.Services
    .AddHealthChecks();

string mongoConnection = configuration.GetValue<string>(configuration.GetSection("MongoConfigurationProvider:ConnectionString").Value);
builder.Configuration.AddMongoProvider(nameof(MongoConfigurationProvider), mongoConnection);

#endregion Service Configuration

WebApplication app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.Run();
