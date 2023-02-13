using Amazon;
using Amazon.Rekognition;
using Amazon.Runtime;
using Doppler.ImageAnalysisApi.Configurations.Amazon;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition;

public static class RekognitionRegistrationExtensions
{
    public static IServiceCollection AddAmazonRekognition(this IServiceCollection services, AmazonConfiguration config)
    {
        services.AddTransient<IRekognitionClient, RekognitionClient>();
        services.AddTransient<IAmazonRekognition, AmazonRekognitionClient>((_) =>
        {
            return new AmazonRekognitionClient(new BasicAWSCredentials(config.AwsAccessKey, config.AwsAccessSecret), RegionEndpoint.GetBySystemName(config.Region));
        });

        return services;
    }
}
