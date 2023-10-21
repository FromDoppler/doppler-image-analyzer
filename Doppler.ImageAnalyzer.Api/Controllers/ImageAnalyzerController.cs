using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doppler.ImageAnalyzer.Api.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ImageAnalyzerController : DopplerControllerBase
{
    protected readonly ILogger<ImageAnalyzerController> _logger;
    protected readonly IImageAnalysisResultRepository _imageAnalysisResultService;

    public ImageAnalyzerController(IMediator mediator, ILogger<ImageAnalyzerController> logger, IImageAnalysisResultRepository imageAnalysisResultService)
        : base(mediator)
    {
        _logger = logger;
        _imageAnalysisResultService = imageAnalysisResultService;
    }

    [HttpGet("{analysisResultId}")]
    public async Task<ActionResult<Response<AnalysisResultResponse>>> GetAnalysisResult([FromRoute] string analysisResultId)
    {
        List<ImageAnalysisResponse>? analysisResult;
        try
        {
            analysisResult = await _imageAnalysisResultService.GetAsync(analysisResultId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected failure obtaining analysis result.");
            return HandleResponse(mapImageAnalysisResult(null, HttpStatusCode.InternalServerError, analysisResultId), "Returned image analysis result");
        }

        if (analysisResult == null)
        {
            return HandleResponse(mapImageAnalysisResult(null, HttpStatusCode.NoContent, analysisResultId), "Returned image analysis result");
        }

        return HandleResponse(mapImageAnalysisResult(analysisResult, HttpStatusCode.OK, analysisResultId), "Returned image analysis result");
    }


    [HttpPost]
    public async Task<ActionResult<Response<AnalysisResultResponse>>> AnalyzeHtml(AnalyzeHtmlRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeHtmlCommand.Command { HtmlToAnalize = request.HtmlToAnalize, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        var resultId = await SaveResultsAsync(response);

        return HandleResponse(mapImageAnalysisResponseList(response, resultId), "Returned image analysis");
    }

    [HttpPost]
    public async Task<ActionResult<Response<AnalysisResultResponse>>> AnalyzeImageList(AnalyzeImageListRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeImageListCommand.Command { ImageUrls = request.ImageUrls, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        var resultId = await SaveResultsAsync(response);

        return HandleResponse(mapImageAnalysisResponseList(response, resultId), "Returned image analysis");
    }

    private async Task<string> SaveResultsAsync(Response<List<ImageAnalysisResponse>> response)
    {
        string resultId = string.Empty;

        if (response.IsSuccessStatusCode && response.Payload != null)
        {
            try
            {
                var imagesAnalysis = response.Payload;
                resultId = await _imageAnalysisResultService.SaveAsync(imagesAnalysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected failure saving analysis response.");
            }
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

        return resultId;
    }

    private Response<AnalysisResultResponse> mapImageAnalysisResponseList(Response<List<ImageAnalysisResponse>> response, string resultId)
    {
        return new Response<AnalysisResultResponse>()
        {
            StatusCode = response.StatusCode,
            ValidationIssue = response.ValidationIssue,
            Payload = response.Payload == null ?
                null :
                new AnalysisResultResponse()
                {
                    AnalysisResult = response.Payload,
                    AnalysisResultId = resultId,
                }
        };
    }

    private Response<AnalysisResultResponse> mapImageAnalysisResult(List<ImageAnalysisResponse>? analysisResult, HttpStatusCode statusCode, string resultId)
    {
        return new Response<AnalysisResultResponse>()
        {
            StatusCode = statusCode,
            Payload = analysisResult == null ?
                null :
                new AnalysisResultResponse()
                {
                    AnalysisResult = analysisResult,
                    AnalysisResultId = resultId,
                }
        };
    }
}
