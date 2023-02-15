using Doppler.ImageAnalysisApi.Helpers.AmazonRekognition.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonRekognition;

public class Rekognition : IRekognition
{
    public float? MinConfidence { get; set; }
}
