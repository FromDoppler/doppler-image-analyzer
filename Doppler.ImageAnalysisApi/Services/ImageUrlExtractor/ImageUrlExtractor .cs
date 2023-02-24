using System.Text.RegularExpressions;

namespace Doppler.ImageAnalysisApi.Services.ImageUrlExtractor;

public partial class ImageUrlExtractor : IImageUrlExtractor
{
    public List<string> Extract(string html)
    {
        List<string> imageUrls = new();

        var matches = ImageUrlRegex().Matches(html).Cast<Match>();

        foreach (Match match in matches)
        {
            string imageUrl = match.Groups[1].Value;
            if (IsValidUrl(imageUrl))
            {
                imageUrls.Add(imageUrl);
            }
        }

        return imageUrls;
    }

    public bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    [GeneratedRegex("<img.+?src=[\"'](.+?)[\"'].*?>")]
    private static partial Regex ImageUrlRegex();
}