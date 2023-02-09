using Amazon.Rekognition.Model;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;

public interface IRekognitionClient
{
    Task<DetectModerationLabelsResponse> DetectModerationLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default);
}
