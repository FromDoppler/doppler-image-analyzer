namespace Doppler.ImageAnalyzer.Api.Services.MongoDB.Interfaces
{
    public interface IImageAnalysisResultService
    {
        Task<string> SaveAsync(int statusCode, List<ImageAnalysisResponse>? imageAnalysisResult, string? errorTitle, string? exceptionMessage);
    }
}
