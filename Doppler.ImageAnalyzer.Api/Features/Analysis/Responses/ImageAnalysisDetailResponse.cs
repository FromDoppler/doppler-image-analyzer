namespace Doppler.ImageAnalyzer.Api.Features.Analysis.Responses;

public class ImageAnalysisDetailResponse
{
    public string? Label { get; set; }
    public float? Confidence { get; set; }
    public bool? IsModeration { get; set; }
}
