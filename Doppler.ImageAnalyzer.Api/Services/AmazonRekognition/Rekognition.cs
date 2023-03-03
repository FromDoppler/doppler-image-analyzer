namespace Doppler.ImageAnalyzer.Api.Services.AmazonRekognition;

public class Rekognition : IRekognition
{
    public float? MinConfidence { get; set; }
    public int? MaxLabels { get; set; }
    public string? ProjectVersionArn { get; set; }
}
