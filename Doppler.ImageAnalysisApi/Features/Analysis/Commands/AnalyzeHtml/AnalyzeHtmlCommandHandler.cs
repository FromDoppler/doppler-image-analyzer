namespace Doppler.ImageAnalysisApi.Features.Analysis.Commands.AnalyzeHtml;

public partial class AnalyzeHtmlCommand
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
                if (string.IsNullOrEmpty(request.HtmlToAnalize))
                {
                    return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("Empty Html.");
                }

                var imageUrls = _imageUrlExtractor.Extract(request.HtmlToAnalize);

                if (imageUrls.Count == 0)
                {
                    return Response.CreateBadRequestResponse<List<ImageAnalysisResponse>>("No images found.");
                }

                var payload = await _analysisOrchestrator.ProcessImageList(imageUrls, request.AnalysisType, cancellationToken);

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
