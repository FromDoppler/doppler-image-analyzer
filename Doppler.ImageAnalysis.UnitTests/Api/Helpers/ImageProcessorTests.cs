using Doppler.ImageAnalysisApi.Configurations.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.ImageDownload.Interfaces;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Api.Helpers
{
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

            var result = await service.ProcessImage("http://filename.jpg", true, CancellationToken.None);

            Assert.True(result == null);
        }
    }
}
