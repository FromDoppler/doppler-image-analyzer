namespace Doppler.ImageAnalyzer.Api.Services.AmazonS3.Interfaces;
public interface IS3File
{
    string? BucketName { get; set; }
    string? Path { get; set; }
    string? FileName { get; set; }
}
