namespace Doppler.ImageAnalysisApi.Services.ImageProcesor;
public class ImageConfidence : IImageConfidence
{
    public string? FileName { get; set; }
    public string? Url { get; set; }
    public string? Label { get; set; }
    public float? Confidence { get; set; }
    public bool? IsModeration { get; set; }
}
