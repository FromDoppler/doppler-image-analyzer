using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalysisApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageAnalyzerController : TaggerControllerBase
    {
        public ImageAnalyzerController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeHtml(string htmlToAnalyze, CancellationToken cancellationToken)
        {
            var command = new AnalyzeHtml.Command { HtmlToAnalize = htmlToAnalyze };
            var response = await _mediator.Send(command, cancellationToken);

            return HandleResponse(response, "Returned image analysis");
        }

        [HttpPost]
        public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeImageList(List<string> imageUrls, CancellationToken cancellationToken)
        {
            var command = new AnalyzeImageList.Command { ImageUrls = imageUrls };
            var response = await _mediator.Send(command, cancellationToken);

            return HandleResponse(response, "Returned image analysis");
        }
    }
}
