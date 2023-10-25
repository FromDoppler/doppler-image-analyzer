﻿namespace Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces
{
    public interface IImageAnalysisResultRepository
    {
        Task<string> SaveAsync(List<ImageAnalysisResponse>? imageAnalysisResult);

        Task<List<ImageAnalysisResponse>?> GetAsync(string? analysisResultId);
    }
}
