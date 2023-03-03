namespace Doppler.ImageAnalyzer.UnitTests.Api.Services;

public class ImageUrlExtractor
{

    [Fact]
    public void ExtractImageUrls_GivenValidHtml_ShouldReturnListOfImageUrls()
    {
        string html = "<html><body><img src='http://www.test.com/image1.jpg' /><img src='http://test.com/image2.jpg' /></body></html>";
        var extractor = new ImageAnalyzer.Api.Services.ImageUrlExtractor.ImageUrlExtractor();

        List<string> imageUrls = extractor.Extract(html);

        Assert.Equal(new List<string> { "http://www.test.com/image1.jpg", "http://test.com/image2.jpg" }, imageUrls);
    }

    [Fact]
    public void ExtractImageUrls_GivenInvalidHtml_ShouldReturnEmptyList()
    {
        string html = "invalid HTML";
        var extractor = new ImageAnalyzer.Api.Services.ImageUrlExtractor.ImageUrlExtractor();

        List<string> imageUrls = extractor.Extract(html);

        Assert.Equal(new List<string>(), imageUrls);
    }

    [Fact]
    public void ExtractImageUrls_GivenHtmlWithEmptyOrInvalidImages_ShouldReturnEmptyList()
    {
        string html = "<html><body><img src='' /><img src='/image1.jpg' /><img src='test.com image2.jpg' /></body></html>";
        var extractor = new ImageAnalyzer.Api.Services.ImageUrlExtractor.ImageUrlExtractor();

        List<string> imageUrls = extractor.Extract(html);

        Assert.Equal(new List<string>(), imageUrls);
    }

    [Fact]
    public void TestIsValidUrl()
    {
        string validHttpUrl = "http://www.google.com";
        var extractor = new ImageAnalyzer.Api.Services.ImageUrlExtractor.ImageUrlExtractor();

        Assert.True(extractor.IsValidUrl(validHttpUrl));

        string validHttpsUrl = "https://www.google.com";

        Assert.True(extractor.IsValidUrl(validHttpsUrl));

        string invalidSchemeUrl = "ftp://www.google.com";

        Assert.False(extractor.IsValidUrl(invalidSchemeUrl));

        string missingSchemeUrl = "www.google.com";

        Assert.False(extractor.IsValidUrl(missingSchemeUrl));

        string emptyUrl = "";

        Assert.False(extractor.IsValidUrl(emptyUrl));

        string invalidCharacterUrl = "ht%tp://www.google.com";

        Assert.False(extractor.IsValidUrl(invalidCharacterUrl));
    }
}
