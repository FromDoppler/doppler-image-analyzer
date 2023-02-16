namespace Doppler.ImageAnalysisApi.Helpers.ImageDownload.Interfaces;

public interface IImageDownloadClient
{
    Task<Stream?> GetImageStream(string url, CancellationToken cancellationToken = default);
}
