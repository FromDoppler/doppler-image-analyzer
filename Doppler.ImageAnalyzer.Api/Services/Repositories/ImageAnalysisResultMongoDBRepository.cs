using Doppler.ImageAnalyzer.Api.Services.Repositories.Entities;
using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories
{
    public class ImageAnalysisResultMongoDBRepository : IImageAnalysisResultRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public ImageAnalysisResultMongoDBRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<BsonDocument>(ImageAnalysisResultDocumentInfo.CollectionName);
        }

        public async Task<string> SaveAsync(List<ImageAnalysisResponse>? imageAnalysisResultList)
        {
            ObjectId _id = ObjectId.GenerateNewId();

            var imageAnalysisResultDocument = imageAnalysisResultList.SerializeToBsonDocument(_id);
            await _collection.InsertOneAsync(document: imageAnalysisResultDocument);

            return _id.ToString();
        }
    }
}
