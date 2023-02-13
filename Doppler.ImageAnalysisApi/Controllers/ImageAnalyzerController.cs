using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Features.Analysis;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalysisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAnalyzerController : TaggerControllerBase
    {
        public ImageAnalyzerController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Response<List<ImageAnalysisResponse>>>> AnalyzeHtml(AnalyzeHtml.Command command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return HandleResponse(response, "Returned image analysis");
        }
    }
}
