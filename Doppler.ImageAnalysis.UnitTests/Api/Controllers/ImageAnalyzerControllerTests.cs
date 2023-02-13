using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Controllers;
using Doppler.ImageAnalysisApi.Features.Analysis;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response>>(), default))
                         .ReturnsAsync(new Response());

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var command = new AnalyzeHtml.Command { HtmlToValidate = html };

            var result = await controller.AnalyzeHtml(command, default);
            
            Assert.NotNull(result);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response>>(), default), Times.Once());
        }
    }
}
