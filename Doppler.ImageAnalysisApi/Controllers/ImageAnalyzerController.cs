using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Features.Analysis.Requests;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalysisApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageAnalyzerController : DopplerControllerBase
    {
        public ImageAnalyzerController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeHtml(AnalyzeHtmlRequest request, CancellationToken cancellationToken)
        {
            var command = new AnalyzeHtml.Command { HtmlToAnalize = request.HtmlToAnalize, AllLabels = request.AllLabels };
            var response = await _mediator.Send(command, cancellationToken);

            return HandleResponse(response, "Returned image analysis");
        }

        [HttpPost]
        public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeImageList(AnalyzeImageListRequest request, CancellationToken cancellationToken)
        {
            var command = new AnalyzeImageList.Command { ImageUrls = request.ImageUrls, AllLabels = request.AllLabels };
            var response = await _mediator.Send(command, cancellationToken);

            return HandleResponse(response, "Returned image analysis");
        }
    }
}
