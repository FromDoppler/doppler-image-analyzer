using System.Net;

namespace Doppler.ImageAnalyzer.Api.Api;

public class Response<T> : Response
{
    public Response()
    {
    }

    public Response(HttpStatusCode statusCode)
        : base(statusCode)
    {
    }

    public T? Payload { get; set; }
}
