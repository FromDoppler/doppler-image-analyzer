﻿using Doppler.ImageAnalysisApi.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doppler.ImageAnalysisApi.Controllers
{
    public abstract class TaggerControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected TaggerControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResponse(Response response, string logMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ValidationIssue);
            }

            return Ok();
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
}
