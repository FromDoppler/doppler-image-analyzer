using Doppler.ImageAnalysisApi.Api;
using Doppler.ImageAnalysisApi.Extensions;
using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;
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
                        var ret = await _imageProcessor.ProcessImage(url, cancellationToken);

                        if (ret != null)
                        {
                            var imageResponse = new ImageAnalysisResponse
                            {
                                ImageUrl = url,
                                AnalysisDetail = new List<ImageAnalysisDetailResponse>()
                            };

                            foreach (var imageAnalysisDetail in ret)
                            {
                                imageResponse.AnalysisDetail.Add(new ImageAnalysisDetailResponse
                                {
                                    Label = imageAnalysisDetail.Label,
                                    Confidence = imageAnalysisDetail.Confidence
                                });
                            }
                            payload.Add(imageResponse);
                        }
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
