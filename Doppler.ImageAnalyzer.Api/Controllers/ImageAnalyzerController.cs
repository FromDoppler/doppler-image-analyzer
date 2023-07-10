using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalyzer.Api.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ImageAnalyzerController : DopplerControllerBase
{
    protected readonly ILogger<ImageAnalyzerController> _logger;

    public ImageAnalyzerController(IMediator mediator, ILogger<ImageAnalyzerController> logger)
        : base(mediator)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeHtml(AnalyzeHtmlRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeHtmlCommand.Command { HtmlToAnalize = request.HtmlToAnalize, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        LogImageAnalysisResponse(response);

        return HandleResponse(response, "Returned image analysis");
    }

    [HttpPost]
    public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeImageList(AnalyzeImageListRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeImageListCommand.Command { ImageUrls = request.ImageUrls, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        LogImageAnalysisResponse(response);

        return HandleResponse(response, "Returned image analysis");
    }

    private void LogImageAnalysisResponse(Response<List<ImageAnalysisResponse>> response)
    {
        if (response.IsSuccessStatusCode && response.Payload != null)
        {
            var imagesAnalysis = response.Payload;
            _logger.LogInformation(
                "Analisys Response => Status Code: {AnalysisResponse_StatusCode} - Number of Images: {AnalysisResponse_ImagesCount} - Analysis Result: {@AnalysisResponse_Result}",
                (int)response.StatusCode,
                imagesAnalysis.Count,
                imagesAnalysis
            );
        }
        else
        {
            _logger.LogInformation(
                "Analisys Response => Status Code: {AnalysisResponse_StatusCode} - Title: {AnalysisResponse_ErrorTitle} - Exception Message: {AnalysisResponse_ExceptionMessage}",
                (int)response.StatusCode,
                response.ValidationIssue.Title,
                response.ValidationIssue.ExceptionMessage
            );
        }
    }
}
