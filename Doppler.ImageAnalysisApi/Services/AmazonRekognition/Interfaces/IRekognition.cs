namespace Doppler.ImageAnalysisApi.Services.AmazonRekognition.Interfaces;

public interface IRekognition
{
    float? MinConfidence { get; set; }
    int? MaxLabels { get; set; }
}
