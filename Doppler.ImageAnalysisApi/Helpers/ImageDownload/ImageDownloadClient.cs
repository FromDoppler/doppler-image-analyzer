using Doppler.ImageAnalysisApi.Helpers.ImageDownload.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.ImageDownload;

public class ImageDownloadClient : IImageDownloadClient
{
    private readonly HttpClient _httpClient;
    public ImageDownloadClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Stream?> GetImageStream(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(url))
            return null;

        var response = await _httpClient.GetAsync(url, cancellationToken);

        return await response.Content.ReadAsStreamAsync();
    }
}
