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

                // TODO: see id generation to use in insertion and return to the user
                var imageAnalysisResultDocument = new BsonDocument
                {
                    // TODO: locate fields names in constants
                    { "_id", _id },
                    { "result", imageAnalysisResultList != null ? ToBsonArray(imageAnalysisResultList) : BsonNull.Value },
                    { "imagesCount", imageAnalysisResultList != null ? imageAnalysisResultList.Count : 0 },
                    { "statusCode", statusCode },
                    { "errorTitle", !string.IsNullOrEmpty(errorTitle) ? errorTitle : BsonNull.Value },
                    { "exceptionMessage", !string.IsNullOrEmpty(exceptionMessage) ? exceptionMessage : BsonNull.Value },
                };

                await _collection.InsertOneAsync(document: imageAnalysisResultDocument);

                return _id.ToString();
            }
            catch (Exception)
            {
                // TODO: treat exception in the controller
                throw;
            }
        }

        private static BsonArray ToBsonArray(List<ImageAnalysisResponse> itemList)
        {
            var bsonArray = new BsonArray();
            foreach (var item in itemList)
            {
                var bsonItem = new BsonDocument
                {
                    // TODO: locate fields names in constants
                    { "imageUrl", item.ImageUrl },
                    { "analysisDetail", item.AnalysisDetail == null ? BsonNull.Value : ToBsonArray(item.AnalysisDetail) }
                };
                bsonArray.Add(bsonItem);
            }

            return bsonArray;
        }

        private static BsonArray ToBsonArray(List<ImageAnalysisDetailResponse> itemList)
        {
            var bsonArray = new BsonArray();
            foreach (var item in itemList)
            {
                var bsonItem = new BsonDocument
                {
                    // TODO: locate fields names in constants
                    { "confidence", item.Confidence },
                    { "label", item.Label },
                    { "isModeration", item.IsModeration }
                };
                bsonArray.Add(bsonItem);
            }

            return bsonArray;
        }
    }
}
