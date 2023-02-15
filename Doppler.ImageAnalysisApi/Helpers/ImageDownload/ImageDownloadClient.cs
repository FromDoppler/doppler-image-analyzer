using Doppler.ImageAnalysisApi.Helpers.Web.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.Web;

public class ImageDownloadClient : IImageDownloadClient
{
    private readonly HttpClient _httpClient;
    public ImageDownloadClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Stream?> GetImageStream(string url, CancellationToken cancellationToken = default)
    {
        return !string.IsNullOrEmpty(url) ? await _httpClient.GetStreamAsync(url, cancellationToken) : null;
    }
}
