using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.ImageAnalysisApi.Api
{
    public class Response<T> : Response
    {
        public Response()
        {
        }

        public Response(HttpStatusCode statusCode)
            : base(statusCode)
        {
        }

        public T Payload { get; set; }
    }
}
