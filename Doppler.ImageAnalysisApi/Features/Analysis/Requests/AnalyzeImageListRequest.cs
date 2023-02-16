namespace Doppler.ImageAnalysisApi.Features.Analysis.Requests
{
    public class AnalyzeImageListRequest
    {
        public List<string>? ImageUrls { get; set; }
        public bool? AllLabels { get; set; }
    }
}
