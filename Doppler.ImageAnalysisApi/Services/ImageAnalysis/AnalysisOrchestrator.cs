namespace Doppler.ImageAnalysisApi.Services.ImageAnalysis;

public class AnalysisOrchestrator : IAnalysisOrchestrator
{
    private readonly IImageProcessor _imageProcessor;

    public AnalysisOrchestrator(IImageProcessor imageProcessor)
    {
        _imageProcessor = imageProcessor;
    }

    public async Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageList, string? analysisType, CancellationToken cancellationToken)
    {
        var analysisResult = new List<ImageAnalysisResponse>();

        if (!Enum.TryParse(analysisType, out AnalysisType enumAnalysisType))
            return analysisResult;

        foreach (var url in imageList)
        {
            var imageConfidences = await _imageProcessor.ProcessImage(url, enumAnalysisType, cancellationToken);

            if (imageConfidences != null)
            {
                var imageResponse = new ImageAnalysisResponse
                {
                    ImageUrl = url,
                    AnalysisDetail = imageConfidences.ToImageAnalysisDetailResponses().ToList()
                };

                analysisResult.Add(imageResponse);
            }
        }

        return analysisResult;
    }
}
