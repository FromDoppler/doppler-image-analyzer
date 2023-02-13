namespace Doppler.ImageAnalysisApi.Features.Analysis.Responses
{
    public class ImageAnalysisResponse
    {
        public string? ImageUrl { get; set; }
        public List<string>? ValidationTags { get; set; }
    }
}
