namespace Doppler.ImageAnalyzer.Api.Features.Analysis.Responses;

public class ImageAnalysisResponse
{
    public string? ImageUrl { get; set; }
    public List<ImageAnalysisDetailResponse>? AnalysisDetail { get; set; }
}
