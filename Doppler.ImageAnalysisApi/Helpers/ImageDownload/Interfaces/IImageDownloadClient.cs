namespace Doppler.ImageAnalysisApi.Helpers.Web.Interfaces;

public interface IImageDownloadClient
{
    Task<Stream?> GetImageStream(string url, CancellationToken cancellationToken = default);
}
