using Amazon.Rekognition;
using Doppler.ImageAnalyzer.Api.Configurations;
using Doppler.ImageAnalyzer.Api.Configurations.Amazon;
using Doppler.ImageAnalyzer.Api.Configurations.Interfaces;
using Doppler.ImageAnalyzer.Api.Services.AmazonRekognition;
using System.Text;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Services;

public class ImageProcessorTests
{
    private readonly Mock<IImageDownloadClient> _imageDownloadClient;
    private readonly Mock<IS3Client> _s3Client;
    private readonly Mock<IRekognitionClient> _rekognitionClient;
    private readonly IAppConfiguration _appConfiguration;

    public ImageProcessorTests()
    {
        _imageDownloadClient = new Mock<IImageDownloadClient>();
        _s3Client = new Mock<IS3Client>();
        _rekognitionClient = new Mock<IRekognitionClient>();
        _appConfiguration = new AppConfiguration();

        var amazonS3Configuration = new AmazonS3Configuration
        {
            BucketName = "bucketName",
            Path = "/",
        };
    
        var amazonRekognition = new AmazonRekognitionConfiguration
        {
            MinConfidence = 50,
            MaxLabels = 10,
            Customlabels = false
        };
        _appConfiguration.AmazonS3 = amazonS3Configuration;
        _appConfiguration.AmazonRekognition = amazonRekognition;
    }

    [Fact]
    public async Task ImageProcessor_GivenNullStream_ShouldReturnNull()
    {
        _imageDownloadClient.Setup(x => x.GetImageStream(It.IsAny<string>(), CancellationToken.None))
                            .ReturnsAsync((Stream)null);

        var service = new ImageProcessor(_imageDownloadClient.Object, _s3Client.Object, _rekognitionClient.Object, _appConfiguration);

        var result = await service.ProcessImage("http://filename.jpg", AnalysisType.AllLabels, CancellationToken.None);

        Assert.True(result == null);
    }

    [Theory]
    [InlineData(AnalysisType.ModerationContent,true, 1)]
    [InlineData(AnalysisType.AllLabels, true, 2)]
    [InlineData(AnalysisType.ModerationContent, false, 1)]
    [InlineData(AnalysisType.AllLabels, false, 2)]
    public async Task ImageProcessor_GivenValidStream_ShouldReturnConfidences(AnalysisType analysisType, bool customlabels, int confidenceCount)
    {
        _appConfiguration.AmazonRekognition.Customlabels = customlabels;
        _imageDownloadClient.Setup(x => x.GetImageStream(It.IsAny<string>(), CancellationToken.None))
                            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes("abc")));

        _s3Client.Setup(x => x.UploadStreamAsync(It.IsAny<Stream>(), It.IsAny<IS3File>(), CancellationToken.None))
                 .Verifiable();

        _rekognitionClient.Setup(x => x.DetectModerationLabelsAsync(It.IsAny<IS3File>(), It.IsAny<IRekognition>(), CancellationToken.None))
                          .ReturnsAsync(new List<ImageConfidence>
                          {
                              new ImageConfidence
                              {
                                  FileName = "filename1",
                                  Url = "http://filename2",
                                  Label = "",
                                  Confidence = 90,
                                  IsModeration = true
                              }
                          });

        _rekognitionClient.Setup(x => x.DetectLabelsAsync(It.IsAny<IS3File>(), It.IsAny<IRekognition>(), CancellationToken.None))
                  .ReturnsAsync(new List<ImageConfidence>
                  {
                              new ImageConfidence
                              {
                                  FileName = "filename2",
                                  Url = "http://filename1",
                                  Label = "",
                                  Confidence = 90,
                                  IsModeration = true
                              }
                  });

        var service = new ImageProcessor(_imageDownloadClient.Object, _s3Client.Object, _rekognitionClient.Object, _appConfiguration);

        var result = await service.ProcessImage("http://filename.jpg", analysisType, CancellationToken.None);

        Assert.False(result == null);
        Assert.True(result.Count() == confidenceCount);
    }
}
