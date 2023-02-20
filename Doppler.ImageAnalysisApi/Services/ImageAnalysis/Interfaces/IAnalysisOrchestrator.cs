namespace Doppler.ImageAnalysisApi.Services.ImageAnalysis.Interfaces;

public interface IAnalysisOrchestrator
{
    Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageList, bool? allLabels, CancellationToken cancellationToken);
}
