using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Helpers;
using Doppler.ImageAnalysisApi.Helpers.ImageAnalysis;
using Moq;
using System.Net;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Logic.Features
{
    public class AnalyzeHtmlTests
    {
        private readonly IImageUrlExtractor _imageUrlExtractor;
        private readonly Mock<IAnalysisOrchestrator> _analysisOrchestrator;
        public AnalyzeHtmlTests()
        {
            _imageUrlExtractor = new ImageUrlExtractor();
            _analysisOrchestrator = new Mock<IAnalysisOrchestrator>();
        }

        [Fact]
        public async Task AnalyzeHtml_GivenEmptyHtml_ShouldReturnBadRequest()
        {
            var command = new AnalyzeHtml.Command { HtmlToAnalize = string.Empty };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor, _analysisOrchestrator.Object);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenHtmlWithoutImages_ShouldReturnBadRequest()
        {
            var html = "<html><div>Your account has been verified.</div></html>";

            var command = new AnalyzeHtml.Command { HtmlToAnalize = html };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor, _analysisOrchestrator.Object);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenAnException_ShouldReturnInternalServerError()
        {
            var imageExtractor = new Mock<IImageUrlExtractor>();
            imageExtractor.Setup(x => x.Extract(It.IsAny<string>())).Throws<InvalidOperationException>();

            var html = "<html><div>Your account has been verified.</div></html>";

            var command = new AnalyzeHtml.Command { HtmlToAnalize = html };
            var handler = new AnalyzeHtml.Handler(imageExtractor.Object, _analysisOrchestrator.Object);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(response.Errors.Count == 1);
        }
    }
}
