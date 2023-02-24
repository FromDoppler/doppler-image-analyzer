namespace Doppler.ImageAnalysisApi.Configurations;

public static class ConfigurationExtensions
{
    public static TConfiguration GetConfiguration<TConfiguration>(this IConfiguration configuration)
            where TConfiguration : new()
    {
        var config = new TConfiguration();
        var configSectionName = typeof(TConfiguration).Name;
        configuration.GetSection(configSectionName).Bind(config);

        return config;
    }
}
