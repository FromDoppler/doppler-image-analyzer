namespace Doppler.ImageAnalyzer.Api.Services.ImageUrlExtractor.Interfaces;

public interface IImageUrlExtractor
{
    List<string> Extract(string html);

    bool IsValidUrl(string url);
}