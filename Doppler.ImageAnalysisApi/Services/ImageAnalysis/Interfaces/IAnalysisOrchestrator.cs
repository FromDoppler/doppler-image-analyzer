using Doppler.ImageAnalyzer.Api.Features.Analysis.Responses;

namespace Doppler.ImageAnalyzer.Api.Services.ImageAnalysis.Interfaces;

public interface IAnalysisOrchestrator
{
    Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageList, string? analysisType, CancellationToken cancellationToken);
}
