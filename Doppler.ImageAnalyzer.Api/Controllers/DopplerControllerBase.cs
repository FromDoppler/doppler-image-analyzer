using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalyzer.Api.Controllers;

public abstract class DopplerControllerBase : ControllerBase
{
    protected readonly IMediator _mediator;

    protected DopplerControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected ActionResult HandleResponse<T>(Response<T> response, string logMessage)
    {
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, response.ValidationIssue);
        }

        return Ok(response.Payload);
    }
}
