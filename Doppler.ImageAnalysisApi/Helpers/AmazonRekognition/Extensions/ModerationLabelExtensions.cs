﻿using Amazon.Rekognition.Model;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Extensions;

public static class ModerationLabelExtensions
{
    public static ImageConfidence ToImageConfidence(this ModerationLabel moderationLabel) => new()
    {
        Confidence = moderationLabel.Confidence,
        Label = moderationLabel.Name
    };

    public static IEnumerable<ImageConfidence> ToImageConfidences(this List<ModerationLabel> moderationLabels)
    {
        return moderationLabels.Select(x => x.ToImageConfidence());
    }
}
