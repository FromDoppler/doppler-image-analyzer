namespace Doppler.ImageAnalyzer.Api.Features.Analysis.Requests;

public class AnalyzeImageListRequest
{
    public List<string>? ImageUrls { get; set; }
    public string? AnalysisType { get; set; }
}
