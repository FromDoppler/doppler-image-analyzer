namespace Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

public interface IImageProcessor
{
    Task<IEnumerable<IImageConfidence>?> ProcessImage(string url, bool? allLabels = false, CancellationToken cancellationToken = default);
}
