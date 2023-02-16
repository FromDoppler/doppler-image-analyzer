using Amazon.Rekognition.Model;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;

public interface IRekognitionClient
{
    Task<IEnumerable<IImageConfidence>> DetectModerationLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
    Task<IEnumerable<IImageConfidence>> DetectLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
}
