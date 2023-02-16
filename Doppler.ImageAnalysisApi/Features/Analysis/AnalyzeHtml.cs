using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Extensions;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;
using Doppler.ImageAnalysisApi.Helpers;
using MediatR;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Extensions;

namespace Doppler.ImageAnalysisApi.Features.Analysis
{
    public class AnalyzeHtml
    {
        public class Command : IRequest<Response<List<ImageAnalysisResponse>>>
        {
            public string? HtmlToAnalize { get; set; }
            public bool? AllLabels { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response<List<ImageAnalysisResponse>>>
        {
            private readonly IImageUrlExtractor _imageUrlExtractor;
            private readonly IImageProcessor _imageProcessor;

            public Handler(IImageUrlExtractor imageUrlExtractor, IImageProcessor imageProcessor)
            {
                _imageUrlExtractor = imageUrlExtractor;
                _imageProcessor = imageProcessor;
            }

            public async Task<Response<List<ImageAnalysisResponse>>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrEmpty(request.HtmlToAnalize))
                    {
                        return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("Empty Html.");
                    }

                    var imageUrls = _imageUrlExtractor.Extract(request.HtmlToAnalize);

                    if (imageUrls.Count == 0)
                    {
                        return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("No images found.");
                    }

                    var payload = new List<ImageAnalysisResponse>();

                    foreach (var url in imageUrls)
                    {
                        var imageConfidences = await _imageProcessor.ProcessImage(url, request.AllLabels, cancellationToken);

                        if (imageConfidences == null)
                        {
                            return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("No Labels Detected");
                        }

                        var imageResponse = new ImageAnalysisResponse
                        {
                            ImageUrl = url,
                            AnalysisDetail = imageConfidences.ToImageAnalysisDetailResponses().ToList()
                        };

                        payload.Add(imageResponse);
                    }

                    return new Response<List<ImageAnalysisResponse>>
                    {
                        Payload = payload
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
