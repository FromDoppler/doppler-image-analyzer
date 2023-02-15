using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonS3;
public class S3File : IS3File
{
    public string? BucketName { get; set; }
    public string? Path { get; set; }
    public string? FileName { get; set; }
}
