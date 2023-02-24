using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.ImageAnalysisApi.Api
{
    public class ResponseErrorDetails : ValidationProblemDetails
    {
        public ResponseErrorDetails()
        {
        }

        public ResponseErrorDetails(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public ResponseErrorDetails(IDictionary<string, string[]> errors)
            : base(errors)
        {
        }

        /// <summary>
        /// The intention of this property is to easily show error exception details in a separate field not consumed by the UI
        /// Developers could check the browser console logs to see the errors details.
        /// </summary>
        public string? ExceptionMessage { get; set; }
    }
}
