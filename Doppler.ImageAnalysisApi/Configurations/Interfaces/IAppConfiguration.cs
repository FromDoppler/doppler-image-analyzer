using Doppler.ImageAnalysisApi.Configurations.Amazon;

namespace Doppler.ImageAnalysisApi.Configurations.Interfaces;

public interface IAppConfiguration
{
    public AmazonS3Configuration? AmazonS3 { get; set; }
    public AmazonRekognitionConfiguration? AmazonRekognition { get; set; }
}
