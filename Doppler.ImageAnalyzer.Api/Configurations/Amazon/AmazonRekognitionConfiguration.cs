namespace Doppler.ImageAnalyzer.Api.Configurations.Amazon;

public class AmazonRekognitionConfiguration
{
    public float? MinConfidence { get; set; }
    public int? MaxLabels { get; set; }
    public string? ProjectVersionArn { get; set; }
    public bool? Customlabels { get; set; }
}
