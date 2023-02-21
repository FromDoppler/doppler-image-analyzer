namespace Doppler.ImageAnalysisApi.Services.AmazonRekognition;

public class Rekognition : IRekognition
{
    public float? MinConfidence { get; set; }
    public int? MaxLabels { get; set; }
}
