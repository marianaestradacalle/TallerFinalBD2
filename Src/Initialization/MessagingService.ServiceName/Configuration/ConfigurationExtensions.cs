using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using SC.Configuration.Provider.Mongo;

namespace MessagingService.ServiceName.Configuration;
public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddJsonProvider(this IConfigurationBuilder configuration, IWebHostEnvironment environment)
            => configuration
                .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.ApplicationName}.json", optional: true, reloadOnChange: true);

    public static IConfigurationBuilder AddKeyVaultProvider(this ConfigurationManager configuration, IWebHostEnvironment environment)
    {
        if ((configuration["AppSettings:Environment"]) == "Local")
        {            
            configuration.AddAzureKeyVault(new AzureKeyVaultConfigurationOptions(configuration["AzureKeyVaultConfig:KeyVault"], 
                configuration["AzureKeyVaultConfig:AppId"], 
                configuration["AzureKeyVaultConfig:AppSecret"]));
        }
        else
        {
            configuration.AddAzureKeyVault(new Uri(configuration["AzureKeyVaultConfig:KeyVault"]), new DefaultAzureCredential());
        }
        
        return configuration;
    }

    public static T ResolveSecrets<T>(this IConfiguration configuration)
    {
        IConfigurationSection secretsSection = configuration.GetRequiredSection(typeof(T).Name);

        foreach (KeyValuePair<string, string> secret in secretsSection.AsEnumerable().Skip(1))
        {
            string secretValue = configuration.GetValue<string>(secret.Value);

            if (secretValue is null) continue;

            configuration[secret.Key] = secretValue;
        }

        return secretsSection.Get<T>();
    }

    public static IConfigurationBuilder AddMongoProvider(this ConfigurationManager configuration,
     string configSectionName, string mongoCon)
    {
        var settings = new MongoAppsettingsConfiguration();
        configuration.GetSection(configSectionName).Bind(settings);
        settings.ConnectionString = mongoCon;
        configuration.AddMongoConfiguration(options =>
        {
            options.ConnectionString = settings.ConnectionString;
            options.CollectionName = settings.CollectionName;
            options.DatabaseName = settings.DatabaseName;
            options.ReloadOnChange = settings.ReloadOnChange;
        });

        return configuration;
    }
}
