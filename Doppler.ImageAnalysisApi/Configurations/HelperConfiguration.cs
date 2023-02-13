using Doppler.ImageAnalysisApi.Configurations.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3;

namespace Doppler.ImageAnalysisApi.Configurations;

public static class HelperConfiguration
{
    public static IServiceCollection AddOperationsLogic(this IServiceCollection services, IAppConfiguration config)
    {
        services.AddSingleton(_ => config);
        services.AddAmazonS3(config!.Amazon!);
        services.AddAmazonRekognition(config!.Amazon!);

        return services;
    }
}
