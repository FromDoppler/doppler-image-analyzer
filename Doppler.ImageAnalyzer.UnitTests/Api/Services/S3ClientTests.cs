using Amazon.S3;
using Doppler.ImageAnalyzer.Api.Services.AmazonS3;
using System.Text;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Services
{
    public class S3ClientTests
    {
        private readonly Mock<IAmazonS3> _amazonS3;

        public S3ClientTests()
        {
            _amazonS3 = new Mock<IAmazonS3>();
        }

        [Fact]
        public async Task UploadStreamAsync_VerifyMethodExecution_WhenOk()
        {
            _amazonS3.Setup(x => x.UploadObjectFromStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), null, CancellationToken.None))
                     .Verifiable();

            var service = new S3Client(_amazonS3.Object);

            var s3file = new S3File
            {
                BucketName = "bucketName",
                Path = "/",
                FileName = "Filename.jpg"
            };

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("abc"));

            await service.UploadStreamAsync(stream, s3file, CancellationToken.None);

            _amazonS3.Verify(x=> x.UploadObjectFromStreamAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), null, CancellationToken.None), Times.Once());
        }
    }
}
