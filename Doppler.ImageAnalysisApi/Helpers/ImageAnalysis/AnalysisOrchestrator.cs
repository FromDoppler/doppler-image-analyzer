using Doppler.ImageAnalysisApi.Features.Analysis.Responses;
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

        public async Task<List<ImageAnalysisResponse>> ProcessImageList(List<string> imageUrls, bool? allLabels, CancellationToken cancellationToken)
        {
            var analysisResult = new List<ImageAnalysisResponse>();

            foreach (var url in imageUrls)
            {
                var ret = await _imageProcessor.ProcessImage(url, allLabels, cancellationToken);

                if (ret != null)
                {
                    var imageResponse = new ImageAnalysisResponse
                    {
                        ImageUrl = url,
                        AnalysisDetail = new List<ImageAnalysisDetailResponse>()
                    };

                    foreach (var imageAnalysisDetail in ret)
                    {
                        imageResponse.AnalysisDetail.Add(new ImageAnalysisDetailResponse
                        {
                            Label = imageAnalysisDetail.Label,
                            Confidence = imageAnalysisDetail.Confidence
                        });
                    }
                    analysisResult.Add(imageResponse);
                }
            }

            return analysisResult;
        }
    }
}
