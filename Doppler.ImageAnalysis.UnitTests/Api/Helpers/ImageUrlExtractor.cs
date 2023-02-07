using Doppler.ImageAnalysisApi.Helpers;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Api.Helpers
{
    public class ImageUrlExtractor
    {

        [Fact]
        public void ExtractImageUrls_GivenValidHtml_ShouldReturnListOfImageUrls()
        {
            string html = "<html><body><img src='http://www.test.com/image1.jpg' /><img src='http://test.com/image2.jpg' /></body></html>";
            var extractor = new ImageAnalysisApi.Helpers.ImageUrlExtractor();

            List<string> imageUrls = extractor.Extract(html);

             Assert.Equal(new List<string> { "http://www.test.com/image1.jpg", "http://test.com/image2.jpg" }, imageUrls);
        }

    }
}
