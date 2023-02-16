namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;

public interface IRekognition
{
    float? MinConfidence { get; set; }
    int? MaxLabels { get; set; }
}
