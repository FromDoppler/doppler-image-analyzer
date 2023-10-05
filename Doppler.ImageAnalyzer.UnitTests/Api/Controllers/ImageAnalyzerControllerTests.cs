using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Controllers;

public class ImageAnalyzerControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ImageAnalyzerController>> _loggerMock;
    private readonly Mock<IImageAnalysisResultRepository> _imageAnalysisResultRepositoryMock;

    public ImageAnalyzerControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ImageAnalyzerController>>();
        _imageAnalysisResultRepositoryMock = new Mock<IImageAnalysisResultRepository>();
    }

    [Fact]
    public async Task AnalyzeHtml_ShouldCallMediator_WhenSuccess()
    {
        var html = "<html><div>Your account has been verified.</div></html>";

        _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                     .ReturnsAsync(new Response<List<ImageAnalysisResponse>>());

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
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
                     .ReturnsAsync(new Response<List<ImageAnalysisResponse>> { StatusCode = HttpStatusCode.BadRequest });

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
        var request = new AnalyzeHtmlRequest { HtmlToAnalize = html, AnalysisType = "ModerationContent" };

        var result = await controller.AnalyzeHtml(request, default);

        Assert.NotNull(result);
        Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 400);
        _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
    }

    [Fact]
    public async Task AnalyzeHtml_ShouldCallImageAnalysisResultRepositoryAndReturnResultId_WhenSuccess()
    {
        // Arrange
        var html = "<html><div><img src='https://www.test.com/test.jpg'></div></html>";

        var response = new Response<List<ImageAnalysisResponse>>()
        {
            Payload = new List<ImageAnalysisResponse>()
            {
                new ImageAnalysisResponse()
                {
                    ImageUrl = "https://www.test.com/test.jpg",
                    AnalysisDetail = new List<ImageAnalysisDetailResponse>(),
                }
            }
        };

        var saveAsyncResultId = "aResultId";

        _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                     .ReturnsAsync(response);

        _imageAnalysisResultRepositoryMock.Setup(m => m.SaveAsync(It.IsAny<List<ImageAnalysisResponse>>()))
                     .ReturnsAsync(saveAsyncResultId);

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
        var request = new AnalyzeHtmlRequest { HtmlToAnalize = html, AnalysisType = "ModerationContent" };

        // Act
        var result = await controller.AnalyzeHtml(request, default);

        // Assert
        Assert.NotNull(result);
        _imageAnalysisResultRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<List<ImageAnalysisResponse>>()), Times.Once());

        // Obtain the result of the call
        var callResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

        // Obtain "Value" and convert to "AnalysisResultResponse"
        var analysisResultResponse = Assert.IsType<AnalysisResultResponse>(callResult.Value);

        // Assert the result contains the "AnalysisResultId" returned by the Repository
        Assert.True(analysisResultResponse.AnalysisResultId == saveAsyncResultId);
    }

    [Fact]
    public async Task AnalyzeHtml_ShouldLogErrorAndReturnEmptyResultId_WhenImageAnalysisResultRepositoryThrowException()
    {
        // Arrange
        var html = "<html><div><img src='https://www.test.com/test.jpg'></div></html>";

        var response = new Response<List<ImageAnalysisResponse>>()
        {
            Payload = new List<ImageAnalysisResponse>()
            {
                new ImageAnalysisResponse()
                {
                    ImageUrl = "https://www.test.com/test.jpg",
                    AnalysisDetail = new List<ImageAnalysisDetailResponse>(),
                }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                     .ReturnsAsync(response);

        _imageAnalysisResultRepositoryMock.Setup(m => m.SaveAsync(It.IsAny<List<ImageAnalysisResponse>>()))
                     .ThrowsAsync(new Exception());

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
        var request = new AnalyzeHtmlRequest { HtmlToAnalize = html, AnalysisType = "ModerationContent" };

        // Act
        var result = await controller.AnalyzeHtml(request, default);

        // Assert
        Assert.NotNull(result);
        _imageAnalysisResultRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<List<ImageAnalysisResponse>>()), Times.Once());
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Unexpected failure saving analysis response."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
            Times.Once
        );

        // Obtain the result of the call
        var callResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

        // Obtain "Value" and convert to "AnalysisResultResponse"
        var analysisResultResponse = Assert.IsType<AnalysisResultResponse>(callResult.Value);

        // Assert the result contains an Empty "AnalysisResultId"
        Assert.True(analysisResultResponse.AnalysisResultId == string.Empty);
    }

    [Fact]
    public async Task AnalyzeImageList_ShouldCallMediator_WhenSuccess()
    {
        var imageUrls = new List<string>{
            "ImageFile.jpg"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default))
                     .ReturnsAsync(new Response<List<ImageAnalysisResponse>>());

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
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

        var controller = new ImageAnalyzerController(_mediatorMock.Object, _loggerMock.Object, _imageAnalysisResultRepositoryMock.Object);
        var request = new AnalyzeImageListRequest { ImageUrls = imageUrls, AnalysisType = "ModerationContent" };

        var result = await controller.AnalyzeImageList(request, default);

        Assert.NotNull(result);
        Assert.True((result.Result as Microsoft.AspNetCore.Mvc.ObjectResult)!.StatusCode == 400);
        _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<Response<List<ImageAnalysisResponse>>>>(), default), Times.Once());
    }
}
