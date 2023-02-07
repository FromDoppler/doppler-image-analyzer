namespace Doppler.ImageAnalysisApi.Helpers
{
    public interface IImageUrlExtractor
    {
        List<string> Extract(string html);

        bool IsValidUrl(string url);
    }
}