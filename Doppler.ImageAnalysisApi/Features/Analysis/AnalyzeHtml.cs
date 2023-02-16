using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Extensions;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers;
using Doppler.ImageAnalysisApi.Helpers.ImageAnalysis;
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
            private readonly IAnalysisOrchestrator _analysisOrchestrator;

            public Handler(IImageUrlExtractor imageUrlExtractor, IAnalysisOrchestrator analysysOrchestrator)
            {
                _imageUrlExtractor = imageUrlExtractor;
                _analysisOrchestrator = analysysOrchestrator;
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

                    var payload = await _analysisOrchestrator.ProcessImageList(imageUrls, request.AllLabels, cancellationToken); 

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
