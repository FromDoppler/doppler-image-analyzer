using Doppler.ImageAnalysisApi.Configurations.Amazon;

namespace Doppler.ImageAnalysisApi.Configurations.Interfaces;

public interface IAppConfiguration
{
    AmazonConfiguration? Amazon { get; set; }
    AmazonS3Configuration? AmazonS3 { get; set; }
    AmazonRekognitionConfiguration? AmazonRekognition { get; set; }
}
