﻿namespace Doppler.ImageAnalysisApi.Configurations.Amazon;

public class AmazonRekognitionConfiguration
{
    public float? MinConfidence { get; set; }
    public int? MaxLabels { get; set;}
}
