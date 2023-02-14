namespace Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

public interface IImageProcessor
{
    Task<IEnumerable<IImageConfidence>?> ProcessImage(string url, CancellationToken cancellationToken = default);
}
