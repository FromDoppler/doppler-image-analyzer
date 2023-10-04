using Doppler.ImageAnalyzer.Api.Services.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Services.Repositories
{
    public class ImageAnalysisResultMongoDBRepositoryTest
    {
        private static ImageAnalysisResultMongoDBRepository CreateSut(IMongoDatabase? database = null)
        {
            return new ImageAnalysisResultMongoDBRepository(database ?? Mock.Of<IMongoDatabase>());
        }

        [Fact]
        public async Task SaveAsync_Should_Throws_Exception_When_Error_Inserting_Results()
        {
            // Arrange
            var mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();
            var mockMongoDatabase = new Mock<IMongoDatabase>();
            var mockMongoClient = new Mock<IMongoClient>();

            // Configure mockMongoCollection to return a value when InsertOneAsync is called
            mockMongoCollection.Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default))
                .ThrowsAsync(new Exception());

            // Configure mockMongoDatabase to return mockMongoCollection when GetCollection is called
            mockMongoDatabase.Setup(d => d.GetCollection<BsonDocument>(It.IsAny<string>(), null))
                .Returns(mockMongoCollection.Object);

            var sut = CreateSut(mockMongoDatabase.Object);

            // Act
            // Assert
            var result = await Assert.ThrowsAsync<Exception>(() => sut.SaveAsync(200, new List<ImageAnalysisResponse>(), null, null));
        }

        [Fact]
        public async Task SaveAsync_Should_Invokes_InsertOneAsync()
        {
            // Arrange
            var mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();
            var mockMongoDatabase = new Mock<IMongoDatabase>();
            var mockMongoClient = new Mock<IMongoClient>();

            // Configure mockMongoCollection to return a value when InsertOneAsync is called
            mockMongoCollection.Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default))
                .Returns(Task.CompletedTask);

            // Configure mockMongoDatabase to return mockMongoCollection when GetCollection is called
            mockMongoDatabase.Setup(d => d.GetCollection<BsonDocument>(It.IsAny<string>(), null))
                .Returns(mockMongoCollection.Object);

            var imageAnalysisResponse = new List<ImageAnalysisResponse>()
            {
                new ImageAnalysisResponse()
                {
                    ImageUrl = "http://url1.jpg",
                    AnalysisDetail = new List<ImageAnalysisDetailResponse>()
                    {
                        new ImageAnalysisDetailResponse()
                        {
                            Confidence = 99,
                            Label = "test",
                            IsModeration = true,
                        }
                    }
                }
            };

            var sut = CreateSut(mockMongoDatabase.Object);

            // Act
            await sut.SaveAsync(200, imageAnalysisResponse, null, null);

            // Assert
            mockMongoCollection.Verify(
                c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default),
                Times.Once());
        }
    }
}
