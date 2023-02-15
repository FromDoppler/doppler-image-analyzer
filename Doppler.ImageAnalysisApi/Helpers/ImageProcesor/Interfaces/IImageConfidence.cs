namespace Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

public interface IImageConfidence
{
    string? FileName { get; set; }
    string? Url { get; set; }
    string? Label { get; set; }
    float? Confidence { get; set; }
}
