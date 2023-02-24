namespace Doppler.ImageAnalysisApi.Features.Analysis.Requests
{
    public class AnalyzeImageListRequest
    {
        public List<string>? ImageUrls { get; set; }
        public string? AnalysisType { get; set; }
    }
}
