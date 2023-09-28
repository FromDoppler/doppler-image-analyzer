using MongoDB.Bson;

namespace Doppler.ImageAnalyzer.Api.Services.MongoDB
{
    public static class ImageAnalysisResultEntityMapper
    {
        public static BsonDocument ToBsonDocument(this List<ImageAnalysisResponse>? results, ObjectId _id, int statusCode, string? errorTitle, string? exceptionMessage)
        {
            return new BsonDocument
                {
                    // TODO: locate fields names in constants
                    { "_id", _id },
                    { "result", results != null ? results.ToBsonArray() : BsonNull.Value },
                    { "imagesCount", results != null ? results.Count : 0 },
                    { "statusCode", statusCode },
                    { "errorTitle", !string.IsNullOrEmpty(errorTitle) ? errorTitle : BsonNull.Value },
                    { "exceptionMessage", !string.IsNullOrEmpty(exceptionMessage) ? exceptionMessage : BsonNull.Value },
                };
        }

        private static BsonArray ToBsonArray(this List<ImageAnalysisResponse> itemList)
        {
            var bsonArray = new BsonArray();
            foreach (var item in itemList)
            {
                var bsonItem = new BsonDocument
                {
                    // TODO: locate fields names in constants
                    { "imageUrl", item.ImageUrl },
                    { "analysisDetail", item.AnalysisDetail != null ? item.AnalysisDetail.ToBsonArray() : BsonNull.Value }
                };
                bsonArray.Add(bsonItem);
            }

            return bsonArray;
        }

        private static BsonArray ToBsonArray(this List<ImageAnalysisDetailResponse> itemList)
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
