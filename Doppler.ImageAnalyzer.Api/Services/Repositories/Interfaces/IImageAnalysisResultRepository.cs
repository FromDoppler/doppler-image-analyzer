namespace Doppler.ImageAnalyzer.Api.Services.Respositories.Interfaces
{
    public interface IImageAnalysisResultRepository
    {
        Task<string> SaveAsync(int statusCode, List<ImageAnalysisResponse>? imageAnalysisResult, string? errorTitle, string? exceptionMessage);
    }
}
