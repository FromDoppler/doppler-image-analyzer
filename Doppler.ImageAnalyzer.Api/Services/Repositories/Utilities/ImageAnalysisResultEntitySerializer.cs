using Doppler.ImageAnalyzer.Api.Services.Respositories.Entities;
using MongoDB.Bson;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories.Utilities
{
    public static class ImageAnalysisResultEntitySerializer
    {
        public static BsonDocument SerializeToBsonDocument(this List<ImageAnalysisResponse>? results, ObjectId _id, int statusCode, string? errorTitle, string? exceptionMessage)
        {
            return new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Id_PropName, _id },
                    { ImageAnalysisResultDocumentInfo.Result_PropName, results != null ? results.SerializeToBsonArray() : BsonNull.Value },
                    { ImageAnalysisResultDocumentInfo.ImagesCount_PropName, results != null ? results.Count : 0 },
                    { ImageAnalysisResultDocumentInfo.StatusCode_PropName, statusCode },
                    { ImageAnalysisResultDocumentInfo.ErrorTitle_PropName, !string.IsNullOrEmpty(errorTitle) ? errorTitle : BsonNull.Value },
                    { ImageAnalysisResultDocumentInfo.ExceptionMessage_PropName, !string.IsNullOrEmpty(exceptionMessage) ? exceptionMessage : BsonNull.Value },
                };
        }

        private static BsonArray SerializeToBsonArray(this List<ImageAnalysisResponse> itemList)
        {
            var bsonArray = new BsonArray();
            foreach (var item in itemList)
            {
                var bsonItem = new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Result_ImageUrl_PropName, item.ImageUrl },
                    {
                        ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_PropName,
                        item.AnalysisDetail != null ? item.AnalysisDetail.SerializeToBsonArray() : BsonNull.Value
                    }
                };
                bsonArray.Add(bsonItem);
            }

            return bsonArray;
        }

        private static BsonArray SerializeToBsonArray(this List<ImageAnalysisDetailResponse> itemList)
        {
            var bsonArray = new BsonArray();
            foreach (var item in itemList)
            {
                var bsonItem = new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Confidence_PropName, item.Confidence },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Label_PropName, item.Label },
                    { ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_IsModeration_PropName, item.IsModeration }
                };
                bsonArray.Add(bsonItem);
            }

            return bsonArray;
        }
    }
}
