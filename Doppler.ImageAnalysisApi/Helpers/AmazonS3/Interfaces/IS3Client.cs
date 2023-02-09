namespace Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

public interface IS3Client
{
    Task UploadStreamAsync(Stream stream, IS3File file, CancellationToken cancellationToken = default);
}
