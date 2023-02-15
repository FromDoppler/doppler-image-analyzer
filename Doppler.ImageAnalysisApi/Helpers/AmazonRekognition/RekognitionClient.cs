using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition;

public class RekognitionClient : IRekognitionClient
{
    private readonly IAmazonRekognition _amazonRekognition;

    public RekognitionClient(IAmazonRekognition amazonRekognition)
    {
        _amazonRekognition = amazonRekognition;
    }

    public async Task<DetectModerationLabelsResponse> DetectModerationLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default)
    {
        var detectModerationLabelsRequest = new DetectModerationLabelsRequest()
        {
            Image = new Image()
            {
                S3Object = new S3Object()
                {
                    Bucket = file.BucketName,
                    Name = $"{file.Path}/{file.FileName}",
                },
            },
            MinConfidence = rekognition.MinConfidence!.Value,
        };
        return await _amazonRekognition.DetectModerationLabelsAsync(detectModerationLabelsRequest, cancellationToken);
    }
}
