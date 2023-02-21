using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace Doppler.ImageAnalysisApi.Services.AmazonRekognition;

public class RekognitionClient : IRekognitionClient
{
    private readonly IAmazonRekognition _amazonRekognition;

    public RekognitionClient(IAmazonRekognition amazonRekognition)
    {
        _amazonRekognition = amazonRekognition;
    }

    public async Task<IEnumerable<IImageConfidence>> DetectModerationLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default)
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

        var result = await _amazonRekognition.DetectModerationLabelsAsync(detectModerationLabelsRequest, cancellationToken);

        return result.ModerationLabels.ToImageConfidences();
    }

    public async Task<IEnumerable<IImageConfidence>> DetectLabelsAsync(IS3File file, IRekognition rekognition, CancellationToken cancellationToken = default)
    {
        var detectLabelsRequest = new DetectLabelsRequest()
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
            MaxLabels = rekognition.MaxLabels!.Value,
        };

        var result = await _amazonRekognition.DetectLabelsAsync(detectLabelsRequest, cancellationToken);

        return result.Labels.ToImageConfidences();
    }
}
