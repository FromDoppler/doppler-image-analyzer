namespace Doppler.ImageAnalysisApi.Features.Analysis.Requests
{
    public class AnalyzeHtmlRequest
    {
        public string? HtmlToAnalize { get; set; }
        public bool? AllLabels { get; set; }
    }
}
