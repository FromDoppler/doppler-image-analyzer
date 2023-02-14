using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Extensions;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers;
using MediatR;

namespace Doppler.ImageAnalysisApi.Features.Analysis
{
    public class AnalyzeHtml
    {
        public class Command : IRequest<Response<List<ImageAnalysisResponse>>>
        {
            public string? HtmlToAnalize { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response<List<ImageAnalysisResponse>>>
        {
            private readonly IImageUrlExtractor _imageUrlExtractor;

            public Handler(IImageUrlExtractor imageUrlExtractor)
            {
                _imageUrlExtractor = imageUrlExtractor;
            }

            public async Task<Response<List<ImageAnalysisResponse>>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(request.HtmlToAnalize))
                    {
                        return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("Empty Html.");
                    }

                    var images = _imageUrlExtractor.Extract(request.HtmlToAnalize);

                    if (images.Count == 0)
                    {
                        return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("No images found.");
                    }

                    foreach (var item in images)
                    {

                    }

                    return new Response<List<ImageAnalysisResponse>>
                    {

                    };
                }
                catch (Exception ex)
                {
                    return ex.ToResponse<List<ImageAnalysisResponse>>(referenceId: null);
                }
            }
        }
    }
}
