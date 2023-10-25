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
        string loggingMessage = "Returned image analysis result";
        List<ImageAnalysisResponse>? analysisResult;
        try
        {
            analysisResult = await _imageAnalysisResultService.GetAsync(analysisResultId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected failure obtaining analysis result.");
            return HandleResponse
            (
                mapImageAnalysisResponseList
                (
                    analysisResult: null,
                    statusCode: HttpStatusCode.InternalServerError,
                    validationIssue: null,
                    resultId: analysisResultId
                ),
                loggingMessage
            );
        }

        if (analysisResult == null)
        {
            return HandleResponse
            (
                mapImageAnalysisResponseList
                (
                    analysisResult: null,
                    HttpStatusCode.NoContent,
                    validationIssue: null,
                    analysisResultId
                ),
                loggingMessage
            );
        }

        return HandleResponse(mapImageAnalysisResponseList(analysisResult, HttpStatusCode.OK, validationIssue: null, analysisResultId), loggingMessage);
    }


    [HttpPost]
    public async Task<ActionResult<Response<AnalysisResultResponse>>> AnalyzeHtml(AnalyzeHtmlRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeHtmlCommand.Command { HtmlToAnalize = request.HtmlToAnalize, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        var resultId = await SaveResultsAsync(response);

        return HandleResponse(mapResponseOfImageAnalysisResponseList(response, resultId), "Returned image analysis");
    }

    [HttpPost]
    public async Task<ActionResult<Response<AnalysisResultResponse>>> AnalyzeImageList(AnalyzeImageListRequest request, CancellationToken cancellationToken)
    {
        var command = new AnalyzeImageListCommand.Command { ImageUrls = request.ImageUrls, AnalysisType = request.AnalysisType };
        var response = await _mediator.Send(command, cancellationToken);

        var resultId = await SaveResultsAsync(response);

        return HandleResponse(mapResponseOfImageAnalysisResponseList(response, resultId), "Returned image analysis");
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

    private Response<AnalysisResultResponse> mapResponseOfImageAnalysisResponseList(Response<List<ImageAnalysisResponse>> response, string resultId)
    {
        return mapImageAnalysisResponseList(response.Payload, response.StatusCode, response.ValidationIssue, resultId);
    }

    private Response<AnalysisResultResponse> mapImageAnalysisResponseList(List<ImageAnalysisResponse>? analysisResult, HttpStatusCode statusCode, ResponseErrorDetails? validationIssue, string resultId)
    {
        var payload = analysisResult == null ? null : new AnalysisResultResponse
        {
            AnalysisResult = analysisResult,
            AnalysisResultId = resultId,
        };

        var response = new Response<AnalysisResultResponse>
        {
            StatusCode = statusCode,
            Payload = payload,
        };

        if (validationIssue != null)
        {
            response.ValidationIssue = validationIssue;
        }

        return response;
    }
}
