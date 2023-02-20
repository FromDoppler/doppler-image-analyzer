namespace Doppler.ImageAnalysisApi.Configurations;

public static class HelperConfiguration
{
    public static IServiceCollection AddOperationsLogic(this IServiceCollection services, IAppConfiguration config)
    {
        services.AddSingleton(_ => config);
        services.AddAmazonS3(config!.Amazon!);
        services.AddAmazonRekognition(config!.Amazon!);
        services.AddScoped<IAnalysisOrchestrator, AnalysisOrchestrator>();
        services.AddScoped<IImageUrlExtractor, ImageUrlExtractor>();
        services.AddScoped<IImageProcessor, ImageProcessor>();
        services.AddScoped<IImageDownloadClient, ImageDownloadClient>();
        services.AddScoped<IS3Client, S3Client>();
        services.AddScoped<IRekognitionClient, RekognitionClient>();
        services.AddHttpClient();

        return services;
    }
}
