using Doppler.ImageAnalyzer.Api.Services.AmazonS3.Interfaces;
using Doppler.ImageAnalyzer.Api.Services.ImageProcesor.Interfaces;

namespace Doppler.ImageAnalyzer.Api.Services.AmazonRekognition.Interfaces;

public interface IRekognitionClient
{
    Task<IEnumerable<IImageConfidence>> DetectModerationLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
    Task<IEnumerable<IImageConfidence>> DetectLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
    Task<IEnumerable<IImageConfidence>> DetectCustomLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
}
