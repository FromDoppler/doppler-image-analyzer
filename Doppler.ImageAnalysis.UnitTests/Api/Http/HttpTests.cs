using System.Net.Http.Json;

namespace Doppler.ImageAnalysis.UnitTests.Api.Http
{
    public class HttpTests
    {
        [Fact]
        public async Task GET_inexistent_endpoint_should_return_404()
        {
            await using var application = new PlaygroundApplication("Production");

            using var client = application.CreateClient();
            using var response = await client.GetAsync("/not-found");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/ImageAnalyzer/AnalyzeHtml")]
        [InlineData("/api/ImageAnalyzer/AnalyzeImageList")]
        public async Task POST_analyze_endpoint_without_content_should_return_400(string requestUri)
        {
            var content = JsonContent.Create(new {});
            await using var application = new PlaygroundApplication("Production");

            using var client = application.CreateClient();
            using var response = await client.PostAsync(requestUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
