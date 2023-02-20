using Amazon;
using Amazon.Rekognition;
using Amazon.Runtime;

namespace Doppler.ImageAnalysisApi.Services.AmazonRekognition;

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
