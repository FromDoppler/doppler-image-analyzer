using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Controllers;
using Doppler.ImageAnalysisApi.Features.Analysis.Requests;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Services.ImageProcesor.Enums;
using MediatR;
using Moq;
using System.Net;
using Xunit;

namespace Doppler.ImageAnalysis.UnitTests.Api.Controllers
{
    public class ImageAnalyzerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        public ImageAnalyzerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task AnalyzeHtml_ShouldCallMediator_WhenSuccess()
        {
            var html = "<html><div>Your account has been verified.</div></html>";

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>>());

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var request = new AnalyzeHtmlRequest { HtmlToAnalize = html, AnalysisType = "ModerationContent" };

            var result = await controller.AnalyzeHtml(request, default);

            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 200);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }

        [Fact]
        public async Task AnalyzeHtml_ShouldReturnBadRequest_WhenHtmlIsEmpty()
        {
            var html = string.Empty;

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>> { StatusCode = HttpStatusCode.BadRequest});

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var request = new AnalyzeHtmlRequest { HtmlToAnalize = html, AnalysisType = "ModerationContent" };

            var result = await controller.AnalyzeHtml(request, default);

            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 400);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }

        [Fact]
        public async Task AnalyzeImageList_ShouldCallMediator_WhenSuccess()
        {
            var imageUrls = new List<string>{
                "ImageFile.jpg"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>>());

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var request = new AnalyzeImageListRequest { ImageUrls = imageUrls, AnalysisType = "ModerationContent" };

            var result = await controller.AnalyzeImageList(request, default);

            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 200);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }

        [Fact]
        public async Task AnalyzeImageList_ShouldReturnBadRequest_WhenHtmlIsEmpty()
        {
            var imageUrls = new List<string>{
                "ImageFile.jpg"
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>> { StatusCode = HttpStatusCode.BadRequest });

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var request = new AnalyzeImageListRequest { ImageUrls = imageUrls, AnalysisType = "ModerationContent" };

            var result = await controller.AnalyzeImageList(request, default);

            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 400);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }
    }
}
