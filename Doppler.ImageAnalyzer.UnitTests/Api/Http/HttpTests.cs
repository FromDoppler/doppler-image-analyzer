using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Http;

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
    public async Task POST_analyze_endpoint_without_token_should_return_401(string requestUri)
    {
        var content = JsonContent.Create(new { });
        await using var application = new PlaygroundApplication("Production");

        using var client = application.CreateClient();
        using var response = await client.PostAsync(requestUri, content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml")]
    [InlineData("/api/ImageAnalyzer/AnalyzeImageList")]
    public async Task POST_analyze_endpoint_with_valid_SU_token_without_content_should_return_400(string requestUri)
    {
        var content = JsonContent.Create(new { });
        await using var application = new PlaygroundApplication();

        using var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestUsersData.TOKEN_SUPERUSER_EXPIRE_20330518);
        using var response = await client.PostAsync(requestUri, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml")]
    [InlineData("/api/ImageAnalyzer/AnalyzeImageList")]
    public async Task POST_analyze_endpoint_with_valid_user_token_without_content_should_return_400(string requestUri)
    {
        var content = JsonContent.Create(new { });
        await using var application = new PlaygroundApplication();

        using var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestUsersData.TOKEN_TEST1_EXPIRE_20330518);
        using var response = await client.PostAsync(requestUri, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml")]
    [InlineData("/api/ImageAnalyzer/AnalyzeImageList")]
    public async Task POST_analyze_endpoint_with_valid_token_without_content_should_return_400(string requestUri)
    {
        var content = JsonContent.Create(new { });
        await using var application = new PlaygroundApplication();

        using var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestUsersData.TOKEN_EXPIRE_20330518);
        using var response = await client.PostAsync(requestUri, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml", TestUsersData.TOKEN_BROKEN)]
    [InlineData("/api/ImageAnalyzer/AnalyzeImageList", TestUsersData.TOKEN_EMPTY)]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml", TestUsersData.TOKEN_EXPIRE_20961002)]
    [InlineData("/api/ImageAnalyzer/AnalyzeImageList", TestUsersData.TOKEN_EXPIRE_20010908)]
    [InlineData("/api/ImageAnalyzer/AnalyzeHtml", TestUsersData.TOKEN_TEST1_EXPIRE_20010908)]

    public async Task POST_analyze_endpoint_with_invalid_should_return_401(string requestUri, string token)
    {
        var content = JsonContent.Create(new { });
        await using var application = new PlaygroundApplication();

        using var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.PostAsync(requestUri, content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
