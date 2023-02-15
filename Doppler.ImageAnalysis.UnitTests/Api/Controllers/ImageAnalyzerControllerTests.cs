﻿using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Controllers;
using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>>());

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var command = new AnalyzeHtml.Command { HtmlToAnalize = html };

            var result = await controller.AnalyzeHtml(command, default);
            
            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult).StatusCode == 200);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }

        [Fact]
        public async Task AnalyzeHtml_ShouldReturnBadRequest_WhenHtmlIsEmpty()
        {
            var html = string.Empty;

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                         .ReturnsAsync(new Response<List<ImageAnalysisResponse>> { StatusCode = HttpStatusCode.BadRequest});

            var controller = new ImageAnalyzerController(_mediatorMock.Object);
            var command = new AnalyzeHtml.Command { HtmlToAnalize = html };

            var result = await controller.AnalyzeHtml(command, default);

            Assert.NotNull(result);
            Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult).StatusCode == 400);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
        }
    }
}