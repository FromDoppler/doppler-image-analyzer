using Doppler.ImageAnalyzer.Api.Services.Repositories;
using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Http;

class PlaygroundApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;
    private readonly IImageAnalysisResultRepository? _imageAnalysisResultService;

    public PlaygroundApplication(string environment = "Development", IImageAnalysisResultRepository? imageAnalysisResultService = null)
    {
        _environment = environment;
        _imageAnalysisResultService = imageAnalysisResultService;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        builder.ConfigureServices(services =>
        {
            // TODO: Add mock/test services to the builder here
            if (_imageAnalysisResultService != null)
            {
                services.AddSingleton(x =>
                {
                    return _imageAnalysisResultService;
                });
            }
        });

        return base.CreateHost(builder);
    }
}