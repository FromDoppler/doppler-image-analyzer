using Amazon.Rekognition.Model;

namespace Doppler.ImageAnalyzer.Api.Services.AmazonRekognition.Extensions;

public static class CustomLabelExtensions
{
    public static ImageConfidence ToImageConfidence(this CustomLabel moderationLabel) => new()
    {
        Confidence = moderationLabel.Confidence,
        Label = moderationLabel.Name,
        IsModeration = false
    };

    public static IEnumerable<ImageConfidence> ToImageConfidences(this List<CustomLabel> labels)
    {
        return labels.Select(x => x.ToImageConfidence());
    }
}
