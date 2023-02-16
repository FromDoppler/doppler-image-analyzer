using Doppler.ImageAnalysisApi.Features.Analysis.Responses;

namespace Doppler.ImageAnalysisApi.Helpers.ImageAnalysis
{
    public interface IAnalysisOrchestrator
    {
        Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageList, bool? allLabels, CancellationToken cancellationToken);
    }
}
