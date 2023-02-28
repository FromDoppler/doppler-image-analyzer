namespace Doppler.ImageAnalyzer.Api.Features.Analysis.Commands.AnalyzeImageList;

public partial class AnalyzeImageListCommand
{
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
                if (request.ImageUrls == null || request.ImageUrls.Count == 0)
                {
                    return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("Empty Html.");
                }

                foreach (var imageUrl in request.ImageUrls.ToArray())
                {
                    if (!_imageUrlExtractor.IsValidUrl(imageUrl))
                    {
                        request.ImageUrls.Remove(imageUrl);
                    }
                }

                if (request.ImageUrls.Count == 0)
                {
                    return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("No valid imnage urls to process.");
                }

                var payload = await _analysisOrchestrator.ProcessImageList(request.ImageUrls, request.AnalysisType, cancellationToken);

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
