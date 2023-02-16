using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Extensions;

public static class ImageConfidenceExtensions
{
    public static ImageAnalysisDetailResponse ToImageAnalysisDetailResponse(this IImageConfidence imageConfidence) => new()
    {
        Confidence = imageConfidence.Confidence,
        Label = imageConfidence.Label,
        IsModeration = imageConfidence.IsModeration,
    };

    public static IEnumerable<ImageAnalysisDetailResponse> ToImageAnalysisDetailResponses(this IEnumerable<IImageConfidence> imageConfidences)
    {
        return imageConfidences.Select(x => x.ToImageAnalysisDetailResponse());
    }
}
