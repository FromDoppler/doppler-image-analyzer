using MongoDB.Bson;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories.Entities
{
    public static class ImageAnalysisResultEntitySerializer
    {
        public static BsonDocument SerializeToBsonDocument(this List<ImageAnalysisResponse>? results, ObjectId _id)
        {
            return new BsonDocument
                {
                    { ImageAnalysisResultDocumentInfo.Id_PropName, _id },
                    { ImageAnalysisResultDocumentInfo.Result_PropName, results != null ? results.SerializeToBsonArray() : BsonNull.Value },
                    { ImageAnalysisResultDocumentInfo.ImagesCount_PropName, results != null ? results.Count : 0 },
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

        public static ImageAnalysisResponse DeserializeBsonValueToImageAnalysisResponse(BsonValue resultItem)
        {
            var imageAnalysisResponse = new ImageAnalysisResponse();

            if (resultItem.IsBsonDocument)
            {
                var resultItemDocument = resultItem.AsBsonDocument;

                string resultFieldName = ImageAnalysisResultDocumentInfo.Result_ImageUrl_PropName;
                if (resultItemDocument.Contains(resultFieldName) && !resultItemDocument[resultFieldName].IsBsonNull)
                {
                    imageAnalysisResponse.ImageUrl = resultItemDocument[resultFieldName].AsString;
                }

                string analysisDetailFieldName = ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_PropName;
                imageAnalysisResponse.AnalysisDetail = resultItemDocument.Contains(analysisDetailFieldName) && !resultItemDocument[analysisDetailFieldName].IsBsonNull ?
                    resultItemDocument[analysisDetailFieldName].AsBsonArray
                        .Select(DeserializeBsonValueToImageAnalysisDetailResponse)
                        .ToList() : null;
            }

            return imageAnalysisResponse;
        }

        private static ImageAnalysisDetailResponse DeserializeBsonValueToImageAnalysisDetailResponse(BsonValue analysisDetailItem)
        {
            var imageAnalysisDetailResponse = new ImageAnalysisDetailResponse();

            if (analysisDetailItem.IsBsonDocument)
            {
                var analysisDetailItemDocument = analysisDetailItem.AsBsonDocument;

                string isModerationFieldName = ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_IsModeration_PropName;
                if (analysisDetailItemDocument.Contains(isModerationFieldName) && !analysisDetailItemDocument[isModerationFieldName].IsBsonNull)
                {
                    imageAnalysisDetailResponse.IsModeration = analysisDetailItemDocument[isModerationFieldName].AsBoolean;
                }

                string labelFieldName = ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Label_PropName;
                if (analysisDetailItemDocument.Contains(labelFieldName) && !analysisDetailItemDocument[labelFieldName].IsBsonNull)
                {
                    imageAnalysisDetailResponse.Label = analysisDetailItemDocument[labelFieldName].AsString;
                }

                string confidenceFieldName = ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Confidence_PropName;
                if (analysisDetailItemDocument.Contains(confidenceFieldName) && !analysisDetailItemDocument[confidenceFieldName].IsBsonNull)
                {
                    var confidenceValue = analysisDetailItemDocument[ImageAnalysisResultDocumentInfo.Result_AnalysisDetail_Confidence_PropName];
                    if (confidenceValue.BsonType == BsonType.Double)
                    {
                        imageAnalysisDetailResponse.Confidence = Convert.ToSingle(confidenceValue.AsDouble);
                    }
                }
            }

            return imageAnalysisDetailResponse;
        }
    }
}
