using SC.Configuration.Provider.Mongo;

namespace GrpcApiService.ServiceName.Configuration;
public static class ConfigurationExtensions
{
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
