using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Helpers;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Logic.Features
{
    public class AnalyzeHtmlTests
    {
        private readonly IImageUrlExtractor _imageUrlExtractor;
        public AnalyzeHtmlTests()
        {
            _imageUrlExtractor = new ImageUrlExtractor();
        }

        [Fact]
        public async Task AnalyzeHtml_GivenEmptyHtml_ShouldReturnBadRequest()
        {
            var command = new AnalyzeHtml.Command { HtmlToValidate = string.Empty };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnalyzeHtml_GivenHtmlWithoutImages_ShouldReturnBadRequest()
        {
            var html = "<html><div>Your account has been verified.</div></html>";

            var command = new AnalyzeHtml.Command { HtmlToValidate = html };
            var handler = new AnalyzeHtml.Handler(_imageUrlExtractor);

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

            var command = new AnalyzeHtml.Command { HtmlToValidate = html };
            var handler = new AnalyzeHtml.Handler(imageExtractor.Object);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(response.Errors.Count == 1);
        }
    }
}
