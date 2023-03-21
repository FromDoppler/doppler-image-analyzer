using Doppler.ImageAnalyzer.Api.Services.ImageProcesor.Enums;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

namespace Doppler.ImageAnalyzer.Api.Services.ImageProcesor;

public class ImageProcessor : IImageProcessor
{
    private readonly IImageDownloadClient _imageDownloadClient;
    private readonly IS3Client _s3Client;
    private readonly IRekognitionClient _rekognitionClient;
    private readonly IAppConfiguration _appConfiguration;

    public ImageProcessor(IImageDownloadClient imageDownloadClient, IS3Client s3Client, IRekognitionClient rekognitionClient, IAppConfiguration appConfiguration)
    {
        _imageDownloadClient = imageDownloadClient;
        _s3Client = s3Client;
        _rekognitionClient = rekognitionClient;
        _appConfiguration = appConfiguration;
    }
    public async Task<IEnumerable<IImageConfidence>?> ProcessImage(string url, AnalysisType? analysisType = AnalysisType.ModerationContent, CancellationToken cancellationToken = default)
    {
        var stream = await _imageDownloadClient.GetImageStream(url, cancellationToken);
        var extension = Path.GetExtension(url);
        IEnumerable<IImageConfidence>? fileConfidences = new List<IImageConfidence>();
        const string gifExtension = ".gif";
        const string convertExtension = ".png";

        if (Equals(stream, null))
            return null;

        using Image image = await Image.LoadAsync(stream, cancellationToken);

        if (extension == gifExtension)
        {
            for (int i = 0; i < image.Frames.Count; i++)
            {
                var frame = image.Frames.CloneFrame(i);

                MemoryStream ms = new();
                frame.SaveAsPng(ms);

                string fileName = $"{Guid.NewGuid()}{convertExtension}";
                var frameConfidences = await ProcessSingleFile(ms, fileName, analysisType, cancellationToken);
                fileConfidences = frameConfidences == null ? fileConfidences : fileConfidences.Union(frameConfidences);
            }
        }
        else
        {
            string fileName = $"{Guid.NewGuid()}{extension}";
            fileConfidences = await ProcessSingleFile(stream, fileName, analysisType, cancellationToken);
        }

        return fileConfidences;
    }

    private async Task<IEnumerable<IImageConfidence>?> ProcessSingleFile(Stream? stream, string fileName, AnalysisType? analysisType = AnalysisType.ModerationContent, CancellationToken cancellationToken = default)
    {
        await UploadStreamAsync(stream!, fileName, cancellationToken);

        IEnumerable<IImageConfidence> confidences = await GetModerationLabels(fileName, cancellationToken);

        _ = _appConfiguration.AmazonRekognition!.Customlabels!.Value ? confidences.Union(await GetCustomLabels(fileName, cancellationToken)) : confidences;

        return analysisType == AnalysisType.AllLabels ? confidences.Union(await GetAllLabels(fileName, cancellationToken)) : confidences;
    }

    private async Task UploadStreamAsync(Stream? stream, string fileName, CancellationToken cancellationToken = default)
    {
        await _s3Client.UploadStreamAsync(stream!, new S3File()
        {
            BucketName = _appConfiguration.AmazonS3!.BucketName,
            Path = _appConfiguration.AmazonS3!.Path,
            FileName = fileName
        }, cancellationToken);
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
            MinConfidence = _appConfiguration.AmazonRekognition!.MinConfidence,
            MaxLabels = _appConfiguration.AmazonRekognition!.MaxLabels,
        }, cancellationToken);
    }

    private async Task<IEnumerable<IImageConfidence>> GetCustomLabels(string fileName, CancellationToken cancellationToken = default)
    {
        return await _rekognitionClient.DetectCustomLabelsAsync(new S3File()
        {
            BucketName = _appConfiguration.AmazonS3!.BucketName,
            Path = _appConfiguration.AmazonS3!.Path,
            FileName = fileName,
        },
        new Rekognition()
        {
            MinConfidence = _appConfiguration.AmazonRekognition!.MinConfidence,
            MaxLabels = _appConfiguration.AmazonRekognition!.MaxLabels,
            ProjectVersionArn = _appConfiguration.AmazonRekognition.ProjectVersionArn,
        }, cancellationToken);
    }
}
