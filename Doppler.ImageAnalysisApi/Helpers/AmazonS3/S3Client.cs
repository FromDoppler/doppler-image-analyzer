using Amazon.S3;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonS3;

public class S3Client : IS3Client
{
    private readonly IAmazonS3 _amazonS3;
    public S3Client(IAmazonS3 amazonS3)
    {
        _amazonS3= amazonS3;
    }

    public async Task UploadStreamAsync(Stream stream, IS3File file, CancellationToken cancellationToken = default)
    {
        await _amazonS3.UploadObjectFromStreamAsync(file.BucketName, $"{file.Path}/{file.FileName}", stream, null, cancellationToken);
    }
}
