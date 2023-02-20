using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Helpers.ImageAnalysis;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;
using Doppler.ImageAnalysisApi.Helpers;
using Moq;
using System.Net;
using Xunit;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor;

namespace Doppler.ImageAnalysis.UnitTests.Logic.Features
{
    public class AnalysisImageListTests
    {
        private readonly IImageUrlExtractor _imageUrlExtractor;
        private readonly IAnalysisOrchestrator _analysisOrchestrator;
        private readonly Mock<IImageProcessor> _imageProcessor;

        public AnalysisImageListTests()
        {
            _imageUrlExtractor = new ImageUrlExtractor();
            _imageProcessor = new Mock<IImageProcessor>();
            _analysisOrchestrator = new AnalysisOrchestrator(_imageProcessor.Object);
        }

        [Fact]
        public async Task AnalyzeImageList_GivenNullList_ShouldReturnBadRequest()
        {
            var command = new AnalyzeImageList.Command { ImageUrls = null, AllLabels = true };
            var handler = new AnalyzeImageList.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeImageList_GivenEmptylList_ShouldReturnBadRequest()
        {
            var command = new AnalyzeImageList.Command { ImageUrls = new List<string>(), AllLabels = true };
            var handler = new AnalyzeImageList.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeImageList_GivenInvalidImages_ShouldReturnBadRequest()
        {
            var command = new AnalyzeImageList.Command { ImageUrls = new List<string> { "abc", "cde" }, AllLabels = true };
            var handler = new AnalyzeImageList.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeImageList_GivenAnException_ShouldReturnInternalServerError()
        {
            var imageExtractor = new Mock<IImageUrlExtractor>();
            imageExtractor.Setup(x => x.IsValidUrl(It.IsAny<string>()))
                          .Throws<InvalidOperationException>();

            var command = new AnalyzeImageList.Command { ImageUrls = new List<string> { "abc", "cde" }, AllLabels = true };
            var handler = new AnalyzeImageList.Handler(imageExtractor.Object, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(response.Errors.Count == 1);
        }

        [Fact]
        public async Task AnalyzeImageList_GivenValidImages_ShouldReturnResponseWithAnalysis()
        {
            _imageProcessor.Setup(x => x.ProcessImage(It.IsAny<string>(), It.IsAny<bool>(), CancellationToken.None))
               .ReturnsAsync(new List<ImageConfidence> { new ImageConfidence { Confidence = (float?)0.99, FileName = "filename.jpg", IsModeration = true, Label = "Label" } });
            var command = new AnalyzeImageList.Command { ImageUrls = new List<string> { "http://filename.jpg"}, AllLabels = false };
            var handler = new AnalyzeImageList.Handler(_imageUrlExtractor, _analysisOrchestrator);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Payload != null);
            Assert.True(response.Payload.Count == 1);
        }
    }
}
