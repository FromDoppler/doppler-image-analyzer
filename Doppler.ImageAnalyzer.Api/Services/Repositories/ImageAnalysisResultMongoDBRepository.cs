using Doppler.ImageAnalyzer.Api.Services.Repositories.Entities;
using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
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

        public async Task<List<ImageAnalysisResponse>?> GetAsync(string? analysisResultId)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;

            if (!ObjectId.TryParse(analysisResultId, out ObjectId _id))
            {
                return null;
            }

            var filter = filterBuilder.Eq(ImageAnalysisResultDocumentInfo.Id_PropName, _id);

            var analysisResultDocument = await (await _collection.FindAsync<BsonDocument>(filter)).SingleOrDefaultAsync();

            if (analysisResultDocument == null)
            {
                return null;
            }

            string resultFieldName = ImageAnalysisResultDocumentInfo.Result_PropName;

            List<ImageAnalysisResponse>? analysisResult = analysisResultDocument.Contains(resultFieldName) && !analysisResultDocument[resultFieldName].IsBsonNull ?
                analysisResultDocument[resultFieldName].AsBsonArray
                    .Select(ImageAnalysisResultEntitySerializer.deserializeBsonValueToImageAnalysisResponse)
                    .ToList() : null;

            return analysisResult;
        }
    }
}
