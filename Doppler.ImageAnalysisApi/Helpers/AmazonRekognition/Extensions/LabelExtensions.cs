using Amazon.Rekognition.Model;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Extensions;

public static class LabelExtensions
{
    public static ImageConfidence ToImageConfidence(this Label moderationLabel) => new()
    {
        Confidence = moderationLabel.Confidence,
        Label = moderationLabel.Name,
        IsModeration = false
    };

    public static IEnumerable<ImageConfidence> ToImageConfidences(this List<Label> labels)
    {
        return labels.Select(x => x.ToImageConfidence());
    }
}
