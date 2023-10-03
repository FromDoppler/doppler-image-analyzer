namespace Doppler.ImageAnalyzer.Api.Services.Respositories.Entities
{
    public static class ImageAnalysisResultDocumentInfo
    {
        // Collection name
        public const string CollectionName = "imageAnalysisResults";

        // Props at root level
        public const string Id_PropName = "_id";
        public const string ImagesCount_PropName = "imagesCount";
        public const string StatusCode_PropName = "statusCode";
        public const string ErrorTitle_PropName = "errorTitle";
        public const string ExceptionMessage_PropName = "exceptionMessage";
        public const string Result_PropName = "result";

        // Props at Result level
        public const string Result_ImageUrl_PropName = "imageUrl";
        public const string Result_AnalysisDetail_PropName = "analysisDetail";

        // Props at Result_AnalysisDetail level
        public const string Result_AnalysisDetail_Confidence_PropName = "confidence";
        public const string Result_AnalysisDetail_Label_PropName = "label";
        public const string Result_AnalysisDetail_IsModeration_PropName = "isModeration";
    }
}
