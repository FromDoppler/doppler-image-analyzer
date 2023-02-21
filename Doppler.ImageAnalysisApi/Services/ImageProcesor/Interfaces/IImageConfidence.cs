﻿namespace Doppler.ImageAnalysisApi.Services.ImageProcesor.Interfaces;

public interface IImageConfidence
{
    string? FileName { get; set; }
    string? Url { get; set; }
    string? Label { get; set; }
    float? Confidence { get; set; }
    public bool? IsModeration { get; set; }
}
