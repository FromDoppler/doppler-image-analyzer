using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Helpers;
using Doppler.ImageAnalysisApi.Helpers.ImageAnalysis;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;
using Moq;
using System.Net;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Logic.Features
{
    public class AnalyzeHtmlTests
    {
        private readonly IImageUrlExtractor _imageUrlExtractor;
        private readonly IAnalysisOrchestrator _analysisOrchestrator;
        private readonly Mock<IImageProcessor> _imageProcessor;
        public AnalyzeHtmlTests()
        {
            _imageUrlExtractor = new ImageUrlExtractor();
            _imageProcessor = new Mock<IImageProcessor>();
            _analysisOrchestrator = new AnalysisOrchestrator(_imageProcessor.Object);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenEmptyHtml_ShouldReturnBadRequest()
        {
            var command = new AnalyzeHtml.Command { HtmlToAnalize = string.Empty };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenHtmlWithoutImages_ShouldReturnBadRequest()
        {
            var html = "<html><div>Your account has been verified.</div></html>";

            var command = new AnalyzeHtml.Command { HtmlToAnalize = html };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor, _analysisOrchestrator);

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
            var handler = new AnalyzeHtml.Handler(imageExtractor.Object, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(response.Errors.Count == 1);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenHtmlWithImages_ShouldReturnResponseWithAnalysis()
        {
            var html = "<html>" +
                "             <div>Your account has been verified.</div>" +
                "             <img id='CDSHBJUdsagy' src='https://img.freepik.com/free-photo/careless-rude-girl-showing-middle-finger-person_176420-21631.jpg'>" +
                "       </html>";

            _imageProcessor.Setup(x => x.ProcessImage(It.IsAny<string>(), It.IsAny<bool>(), CancellationToken.None))
                           .ReturnsAsync(new List<ImageConfidence> { new ImageConfidence { Confidence = (float?)0.99, FileName = "filename.jpg", IsModeration = false, Label = "Label" } });
            var command = new AnalyzeHtml.Command { HtmlToAnalize = html, AllLabels = true };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Payload != null);
            Assert.True(response.Payload.Count == 1);
        }
    }
}
