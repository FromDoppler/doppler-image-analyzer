using System.Net;

namespace Doppler.ImageAnalysisApi.Extensions
{
    public static class ExceptionExtensions
    {
        public const string UnexpectedErrorKey = "UNEXPECTED_ERROR";

        public static Response<T> ToResponse<T>(this Exception exception, string? referenceId = null)
        {
            var description = referenceId != null
                ? $"ReferenceId: {referenceId}"
                : exception.ToString(); // Kept for backward compatibility.
            return Response.GetResponseError<T>(statusCode: HttpStatusCode.InternalServerError, errorKey: UnexpectedErrorKey, errorDescription: description, exception: exception);
        }

        public static Response ToResponse(this Exception exception, string? referenceId = null)
        {
            var description = referenceId != null
                ? $"ReferenceId: {referenceId}"
                : exception.ToString(); // Kept for backward compatibility.
            return Response.GetResponseError(statusCode: HttpStatusCode.InternalServerError, errorKey: UnexpectedErrorKey, errorDescription: description, exception: exception);
        }
    }
}
