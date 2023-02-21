using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Extensions;
using Doppler.ImageAnalysisApi.Helpers.ImageProcesor.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.ImageAnalysis
{
    public class AnalysisOrchestrator : IAnalysisOrchestrator
    {
        private readonly IImageProcessor _imageProcessor;

        public AnalysisOrchestrator(IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }

        public async Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageList, bool? allLabels, CancellationToken cancellationToken)
        {
            var analysisResult = new List<ImageAnalysisResponse>();

            foreach (var url in imageList)
            {
                var imageConfidences = await _imageProcessor.ProcessImage(url, allLabels, cancellationToken);

                if (imageConfidences != null)
                {
                    var imageResponse = new ImageAnalysisResponse
                    {
                        ImageUrl = url,
                        AnalysisDetail = imageConfidences.ToImageAnalysisDetailResponses().ToList()
                    };

                    analysisResult.Add(imageResponse);
                }
            }

            return analysisResult;
        }
    }
}
