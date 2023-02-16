using Doppler.ImageAnalysisApi.Configurations.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.Web.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.ImageProcesor;

public class ImageProcessor : IImageProcessor
{
    private readonly IImageDownloadClient _imageDownloadClient;
    private readonly IS3Client _s3Client;
    private readonly IRekognitionClient _rekognitionClient;
    private readonly IAppConfiguration _appConfiguration;

    public ImageProcessor(IImageDownloadClient imageDownloadClient, IS3Client s3Client, IRekognitionClient rekognitionClient, IAppConfiguration appConfiguration)
    {
        _imageDownloadClient= imageDownloadClient;
        _s3Client = s3Client;
        _rekognitionClient = rekognitionClient;
        _appConfiguration = appConfiguration;
    }
    public async Task<IEnumerable<IImageConfidence>?> ProcessImage(string url, bool allLabels = false, CancellationToken cancellationToken = default)
    {
        var stream = await _imageDownloadClient.GetImageStream(url, cancellationToken);
        var extension = Path.GetExtension(url);

        if (Equals(stream, null))
            return null;

        string fileName = $"{Guid.NewGuid()}{extension}";

        await _s3Client.UploadStreamAsync(stream, new S3File()
        {
            BucketName = _appConfiguration.AmazonS3!.BucketName,
            Path= _appConfiguration.AmazonS3!.Path,
            FileName = fileName
        }, cancellationToken);

        return allLabels ? await GetAllLabels(fileName, cancellationToken) :
            await GetModerationLabels(fileName, cancellationToken); 
    }

    private async Task<IEnumerable<IImageConfidence>> GetModerationLabels(string fileName, CancellationToken cancellationToken = default)
    {
        return await _rekognitionClient.DetectModerationLabelsAsync(new S3File()
        {
            BucketName = _appConfiguration.AmazonS3!.BucketName,
            Path = _appConfiguration.AmazonS3!.Path,
            FileName = fileName,
        },
        new Rekognition()
        {
            MinConfidence = _appConfiguration.AmazonRekognition!.MinConfidence
        }, cancellationToken);
    }

    private async Task<IEnumerable<IImageConfidence>> GetAllLabels(string fileName, CancellationToken cancellationToken = default)
    {
        return await _rekognitionClient.DetectLabelsAsync(new S3File()
        {
            BucketName = _appConfiguration.AmazonS3!.BucketName,
            Path = _appConfiguration.AmazonS3!.Path,
            FileName = fileName,
        },
        new Rekognition()
        {
            MinConfidence = _appConfiguration.AmazonRekognition!.MinConfidence
        }, cancellationToken);
    }
}
