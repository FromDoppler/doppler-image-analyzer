using Doppler.ImageAnalyzer.Api.Services.MongoDB.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.MongoDB
{
    public class ImageAnalysisResultService : IImageAnalysisResultService
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public ImageAnalysisResultService(IMongoClient mongoClient, IOptions<ImageAnalyzerMongoDBContextSettings> mongoContextSettings)
        {
            var database = mongoClient.GetDatabase(mongoContextSettings.Value.DatabaseName);
            // TODO: locate collection names in constants
            _collection = database.GetCollection<BsonDocument>("imageAnalysisResults");
        }

        public async Task<string> SaveAsync(int statusCode, List<ImageAnalysisResponse>? imageAnalysisResultList, string? errorTitle, string? exceptionMessage)
        {
            try
            {
                ObjectId _id = ObjectId.GenerateNewId();

                var imageAnalysisResultDocument = imageAnalysisResultList.ToBsonDocument(_id, statusCode, errorTitle, exceptionMessage);
                await _collection.InsertOneAsync(document: imageAnalysisResultDocument);

                return _id.ToString();
            }
            catch (Exception)
            {
                // TODO: treat exception in the controller
                throw;
            }
        }
    }
}
