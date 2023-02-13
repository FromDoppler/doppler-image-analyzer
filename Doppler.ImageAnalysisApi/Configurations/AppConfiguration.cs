using Doppler.ImageAnalysisApi.Configurations.Amazon;
using Doppler.ImageAnalysisApi.Configurations.Interfaces;

namespace Doppler.ImageAnalysisApi.Configurations;

public class AppConfiguration : IAppConfiguration
{
    public AmazonConfiguration? Amazon { get ; set ; }
    public AmazonS3Configuration? AmazonS3 { get ; set ; }
    public AmazonRekognitionConfiguration? AmazonRekognition { get ; set ; }
}
