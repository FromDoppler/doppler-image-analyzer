using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.ImageAnalysisApi.Api
{
    public class Response
    {
        protected const string DefaultErrorTitle = "Something went wrong";

        protected static readonly Dictionary<HttpStatusCode, string> _errorTitlesByCode = new()
        {
            { HttpStatusCode.BadRequest, "One or more validation errors occurred" },
            { HttpStatusCode.InternalServerError, DefaultErrorTitle },
            { HttpStatusCode.NotFound, "Resource not found" },
            { HttpStatusCode.PreconditionFailed, "One or more requirements were not satisfied" },
            { HttpStatusCode.Unauthorized, "User must be authenticated" },
            { HttpStatusCode.Locked, "The resource is locked" },
        };

        public Response()
        {
            StatusCode = HttpStatusCode.OK;
            ValidationIssue = new ResponseErrorDetails
            {
                Title = string.Empty
            };
        }

        public Response(HttpStatusCode statusCode)
            : this()
        {
            StatusCode = statusCode;
            if (IsSuccessStatusCode)
            {
                return;
            }

            ValidationIssue.Title = GetErrorTitle(statusCode);
        }

        public IDictionary<string, string[]> Errors
        {
            get { return ValidationIssue.Errors; }
        }

        public bool IsSuccessStatusCode
            => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public HttpStatusCode StatusCode { get; set; }

        public ResponseErrorDetails ValidationIssue { get; set; }

        public static Response CreateBadRequestResponse(string? errorTitle = null)
        {
            return new Response
            {
                ValidationIssue = GetResponseErrorDetails(HttpStatusCode.BadRequest, errorTitle),
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static Response<T> CreateBadRequestResponse<T>(string? errorTitle = null)
        {
            return new Response<T>
            {
                ValidationIssue = GetResponseErrorDetails(HttpStatusCode.BadRequest, errorTitle),
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static Response CreateBadRequestResponse(IDictionary<string, string[]> errorList)
        {
            return new Response
            {
                ValidationIssue = new ResponseErrorDetails(errorList)
                {
                    Title = GetErrorTitle(HttpStatusCode.BadRequest),
                    Status = (int)HttpStatusCode.BadRequest
                },
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static Response<T> CreateBadRequestResponse<T>(IDictionary<string, string[]> errorList)
        {
            return new Response<T>
            {
                ValidationIssue = new ResponseErrorDetails(errorList)
                {
                    Title = GetErrorTitle(HttpStatusCode.BadRequest),
                    Status = (int)HttpStatusCode.BadRequest
                },
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static Response CreateLockErrorResponse(string errorDetail, string? title = null)
        {
            var errors = new Dictionary<string, string[]>()
            {
                { "lockedItem", new string[] { errorDetail } }
            };

            return new Response
            {
                ValidationIssue = new ResponseErrorDetails(errors)
                {
                    Title = title ?? GetErrorTitle(HttpStatusCode.Locked),
                    Status = (int)HttpStatusCode.Locked
                },
                StatusCode = HttpStatusCode.Locked
            };
        }

        public static Response<T> CreateLockErrorResponse<T>(string errorDetail, string? title = null)
        {
            var errors = new Dictionary<string, string[]>()
            {
                { "lockedItem", new string[] { errorDetail } }
            };

            return new Response<T>
            {
                ValidationIssue = new ResponseErrorDetails(errors)
                {
                    Title = title ?? GetErrorTitle(HttpStatusCode.Locked),
                    Status = (int)HttpStatusCode.Locked
                },
                StatusCode = HttpStatusCode.Locked
            };
        }

        public static Response CreateUnauthorizedResponse(string? errorTitle = null)
        {
            return new Response
            {
                ValidationIssue = GetResponseErrorDetails(HttpStatusCode.Unauthorized, errorTitle),
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static Response<T> CreateUnauthorizedResponse<T>(string? errorTitle = null)
        {
            return new Response<T>
            {
                ValidationIssue = GetResponseErrorDetails(HttpStatusCode.Unauthorized, errorTitle),
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static Response GetResponseError(
            HttpStatusCode statusCode,
            string errorKey,
            string errorDescription,
            Exception exception,
            string? errorTitle = null)
        {
            return new Response(statusCode)
            {
                ValidationIssue = GetResponseErrorDetails(statusCode, errorKey, errorDescription, exception, errorTitle),
                StatusCode = statusCode
            };
        }

        public static Response<T> GetResponseError<T>(
            HttpStatusCode statusCode,
            string errorKey,
            string errorDescription,
            Exception exception,
            string? errorTitle = null)
        {
            return new Response<T>(statusCode)
            {
                ValidationIssue = GetResponseErrorDetails(statusCode, errorKey, errorDescription, exception, errorTitle),
                StatusCode = statusCode
            };
        }

        protected static string GetErrorTitle(HttpStatusCode statusCode)
        {
            return !_errorTitlesByCode.ContainsKey(statusCode)
                    ? DefaultErrorTitle
                    : _errorTitlesByCode[statusCode];
        }

        private static ResponseErrorDetails GetResponseErrorDetails(HttpStatusCode statusCode, string errorKey, string errorDescription, Exception ex, string? errorTitle = null)
        {
            return new ResponseErrorDetails(new Dictionary<string, string[]>
            {
                { errorKey, new string[] { errorDescription } }
            })
            {
                Title = errorTitle ?? GetErrorTitle(statusCode),
                Status = (int)statusCode,
                ExceptionMessage = ex.Message
            };
        }

        private static ResponseErrorDetails GetResponseErrorDetails(HttpStatusCode statusCode, string? errorTitle = null)
        {
            return new ResponseErrorDetails()
            {
                Title = errorTitle ?? GetErrorTitle(statusCode),
                Status = (int)statusCode
            };
        }
    }
}
