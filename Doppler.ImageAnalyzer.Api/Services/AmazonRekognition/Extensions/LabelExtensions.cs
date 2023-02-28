using Amazon.Rekognition.Model;

namespace Doppler.ImageAnalyzer.Api.Services.AmazonRekognition.Extensions;

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
