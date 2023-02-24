namespace Doppler.ImageAnalysisApi.Services.ImageProcesor.Interfaces;

public interface IImageProcessor
{
    Task<IEnumerable<IImageConfidence>?> ProcessImage(string url, AnalysisType? analysisType = AnalysisType.ModerationContent, CancellationToken cancellationToken = default);
}
