using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Doppler.ImageAnalyzer.Api.Services.AmazonRekognition;
using Doppler.ImageAnalyzer.Api.Services.AmazonS3;
using System.Xml.Linq;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Services
{
    public class RekognitionClientTests
    {
        private readonly Mock<IAmazonRekognition> _amazonRekognition;
        public RekognitionClientTests()
        {
            _amazonRekognition = new Mock<IAmazonRekognition>();
        }

        [Fact]
        public async Task DetectModerationLabel_ReturnListOfConfidences_WhenOk()
        {
            _amazonRekognition.Setup(x => x.DetectModerationLabelsAsync(It.IsAny<DetectModerationLabelsRequest>(), CancellationToken.None))
                              .ReturnsAsync(new DetectModerationLabelsResponse());

            var service = new RekognitionClient(_amazonRekognition.Object);

            var (s3File, rekognition) = CreateamazonServiceParameters();

            var result = await service.DetectModerationLabelsAsync(s3File, rekognition, CancellationToken.None);

            Assert.True(result != null);
        }

        [Fact]
        public async Task DetectLabels_ReturnListOfConfidences_WhenOk()
        {
            _amazonRekognition.Setup(x=> x.DetectLabelsAsync(It.IsAny<DetectLabelsRequest>(), CancellationToken.None))
                              .ReturnsAsync(new DetectLabelsResponse());
            var service = new RekognitionClient(_amazonRekognition.Object);

            var (s3File, rekognition) = CreateamazonServiceParameters();

            var result = await service.DetectLabelsAsync(s3File, rekognition, CancellationToken.None);

            Assert.True(result != null);
        }

        [Fact]
        public async Task DetectCustomLabelsAsync_ReturnListOfConfidences_WhenOk()
        {
            _amazonRekognition.Setup(x => x.DetectCustomLabelsAsync(It.IsAny<DetectCustomLabelsRequest>(), CancellationToken.None))
                              .ReturnsAsync(new DetectCustomLabelsResponse
                              {
                                  CustomLabels = new List<CustomLabel> {
                                                                          new CustomLabel {
                                                                              Confidence = 90,
                                                                              Name = "label1"
                                                                          },
                                                                          new CustomLabel {
                                                                              Confidence = 90,
                                                                              Name = "label2"
                                                                          }
                                  }
                              });

            var service = new RekognitionClient(_amazonRekognition.Object);

            var (s3File, rekognition) = CreateamazonServiceParameters();

            var result = await service.DetectCustomLabelsAsync(s3File, rekognition, CancellationToken.None);

            Assert.True(result != null);
        }

        private static (S3File, Rekognition) CreateamazonServiceParameters()
        {
            var s3file = new S3File
            {
                BucketName = "bucketName",
                Path = "/",
                FileName = "Filename.jpg"
            };
            var rekognition = new Rekognition
            {
                MinConfidence = 90,
                MaxLabels = 10,
                ProjectVersionArn = ""
            };

            return new(s3file, rekognition);
        }
    }
}
