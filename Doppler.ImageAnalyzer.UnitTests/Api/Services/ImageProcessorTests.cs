﻿namespace Doppler.ImageAnalyzer.UnitTests.Api.Services;

public class ImageProcessorTests
{
    private readonly Mock<IImageDownloadClient> _imageDownloadClient;
    private readonly Mock<IS3Client> _s3Client;
    private readonly Mock<IRekognitionClient> _rekognitionClient;
    private readonly Mock<IAppConfiguration> _appConfiguration;

    public ImageProcessorTests()
    {
        _imageDownloadClient = new Mock<IImageDownloadClient>();
        _s3Client = new Mock<IS3Client>();
        _rekognitionClient = new Mock<IRekognitionClient>();
        _appConfiguration = new Mock<IAppConfiguration>();
    }

    [Fact]
    public async Task ImageProcessor_GivenNullStream_ShouldReturnNull()
    {
        _imageDownloadClient.Setup(x => x.GetImageStream(It.IsAny<string>(), CancellationToken.None))
                            .ReturnsAsync((Stream)null);

        var service = new ImageProcessor(_imageDownloadClient.Object, _s3Client.Object, _rekognitionClient.Object, _appConfiguration.Object);

        var result = await service.ProcessImage("http://filename.jpg", AnalysisType.AllLabels, CancellationToken.None);

        Assert.True(result == null);
    }
}
