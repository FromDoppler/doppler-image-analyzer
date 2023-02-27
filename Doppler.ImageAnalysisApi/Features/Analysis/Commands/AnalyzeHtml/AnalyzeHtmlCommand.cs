namespace Doppler.ImageAnalyzer.Api.Features.Analysis.Commands.AnalyzeHtml
{
    public partial class AnalyzeHtmlCommand
    {
        public class Command : IRequest<Response<List<ImageAnalysisResponse>>>
        {
            public string? HtmlToAnalize { get; set; }
            public string? AnalysisType { get; set; }
        }
    }
}
