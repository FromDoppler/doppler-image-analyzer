using AutoFixture;
using Doppler.ImageAnalyzer.Api.Services.Repositories;
using Doppler.ImageAnalyzer.Api.Services.Repositories.Entities;
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
        public async Task SaveAsync_Should_Throw_Exception_When_Error_Inserting_Results()
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
            var result = await Assert.ThrowsAsync<Exception>(() => sut.SaveAsync(new List<ImageAnalysisResponse>()));
        }

        [Fact]
        public async Task SaveAsync_Should_Invoke_InsertOneAsync()
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
            await sut.SaveAsync(imageAnalysisResponse);

            // Assert
            mockMongoCollection.Verify(
                c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default),
                Times.Once());
        }

        [Fact]
        public async Task GetAsync_Should_Throw_Exception_When_Error_Happened_Obtaining_AnalysisResult()
        {
            // Arrange
            var mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();
            var mockMongoDatabase = new Mock<IMongoDatabase>();

            mockMongoCollection
                .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default))
                .ThrowsAsync(new Exception());

            mockMongoDatabase.Setup(d => d.GetCollection<BsonDocument>(It.IsAny<string>(), null))
                .Returns(mockMongoCollection.Object);

            var sut = CreateSut(mockMongoDatabase.Object);

            // Act
            // Assert
            var result = await Assert.ThrowsAsync<Exception>(() => sut.GetAsync("65327ddef2788a5272cf5126"));
        }

        [Fact]
        public async Task GetAsync_Should_Return_Null_When_AnalysisResultId_HasInvalidFormat()
        {
            // Arrange
            var invalidId = "_idWithInvalidFormat";

            var sut = CreateSut();

            // Act
            var result = await sut.GetAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Null_When_DB_Returns_Empty_Result()
        {
            // Arrange
            List<BsonDocument> emptyList = new List<BsonDocument>();

            var mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();
            var mockMongoDatabase = new Mock<IMongoDatabase>();

            var mockCursor = new Mock<IAsyncCursor<BsonDocument>>();
            mockCursor
                .Setup(_ => _.Current)
                .Returns(emptyList);

            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            mockMongoCollection
                .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default))
                .ReturnsAsync(mockCursor.Object);

            mockMongoDatabase.Setup(d => d.GetCollection<BsonDocument>(It.IsAny<string>(), null))
                .Returns(mockMongoCollection.Object);

            var sut = CreateSut(mockMongoDatabase.Object);

            // Act
            var result = await sut.GetAsync("65327ddef2788a5272cf5126");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Should_Return_AnalysisResult_When_Id_IsValid_And_Exists()
        {
            // Arrange
            var validAndExistentId = "65327ddef2788a5272cf5126";

            List<string> ids = new List<string> { validAndExistentId };
            List<BsonDocument> fakeDocuments = FakeAnalysisResultDocuments(ids);

            var mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();
            var mockMongoDatabase = new Mock<IMongoDatabase>();

            var mockCursor = new Mock<IAsyncCursor<BsonDocument>>();
            mockCursor
                .Setup(_ => _.Current)
                .Returns(fakeDocuments.Where(x => x["_id"].AsString == validAndExistentId));

            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            mockMongoCollection
                .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default))
                .ReturnsAsync(mockCursor.Object);

            mockMongoDatabase.Setup(d => d.GetCollection<BsonDocument>(It.IsAny<string>(), null))
                .Returns(mockMongoCollection.Object);

            var sut = CreateSut(mockMongoDatabase.Object);

            // Act
            var result = await sut.GetAsync(validAndExistentId);

            // Assert
            Assert.IsType<List<ImageAnalysisResponse>>(result);
            Assert.True(result.Count == 1);
        }

        private static List<BsonDocument> FakeAnalysisResultDocuments(List<string> ids)
        {
            var fakeDocuments = new List<BsonDocument>();

            foreach (var id in ids)
            {
                var resultArray = FakeAnalysisDetailDocuments();

                var document = new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Id_PropName, id },
                    { ImageAnalysisResultDocumentInfo.Result_PropName, resultArray },
                };

                fakeDocuments.Add(document);
            }

            return fakeDocuments;
        }

        private static BsonArray FakeAnalysisDetailDocuments()
        {
            var fixture = new Fixture();
            var analysisDetailArray = new BsonArray
            {
                new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_IsModeration_PropName, fixture.Create<bool>() },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Label_PropName, fixture.Create<string>() },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Confidence_PropName, fixture.Create<double>() },
                },
                new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_IsModeration_PropName, fixture.Create<bool>() },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Label_PropName, fixture.Create<string>() },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Confidence_PropName, fixture.Create<double>() },
                },
            };

            return new BsonArray
            {
                new BsonDocument
                    {
                        { ImageAnalysisResultDocumentInfo.Result_ImageUrl_PropName, fixture.Create<string>() },
                        { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_PropName, analysisDetailArray },
                    }
            };
        }
    }
}
