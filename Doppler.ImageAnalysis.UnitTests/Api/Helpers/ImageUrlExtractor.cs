using Doppler.ImageAnalysisApi.Helpers;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Api.Helpers
{
    public class ImageUrlExtractor
    {

        [Fact]
        public void ExtractImageUrls_GivenValidHtml_ShouldReturnListOfImageUrls()
        {
            string html = "<html><body><img src='image1.jpg' /><img src='image2.jpg' /></body></html>";
            var extractor = new ImageAnalysisApi.Helpers.ImageUrlExtractor();

            List<string> imageUrls = extractor.Extract(html);

             Assert.Equal(new List<string> { "image1.jpg", "image2.jpg" }, imageUrls);
        }

    }
}
