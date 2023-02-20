namespace Doppler.ImageAnalysisApi.Features.Analysis.Commands.AnalyzeImageList;

public partial class AnalyzeImageListCommand
{
    public class Command : IRequest<Response<List<ImageAnalysisResponse>>>
    {
        public List<string>? ImageUrls { get; set; }
        public bool? AllLabels { get; set; }
    }
}
