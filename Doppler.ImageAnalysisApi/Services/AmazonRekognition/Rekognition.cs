namespace Doppler.ImageAnalysisApi.Services.AmazonRekognition;

public class Rekognition : IRekognition
{
    public float? MinConfidence { get; set; }
    public int? MaxLabels { get; set; }
    public string? ProjectVersionArn { get; set; }
    public bool? Customlabels { get; set; }

}
