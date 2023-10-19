using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.UnitTests.Api.Http;

class PlaygroundApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public PlaygroundApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        builder.ConfigureServices(services =>
        {
            // TODO: Add mock/test services to the builder here
            services.AddSingleton(x =>
            {
                Mock<IMongoDatabase> mockMongoDatabase = new Mock<IMongoDatabase>();
                return mockMongoDatabase.Object;
            });
        });

        return base.CreateHost(builder);
    }
}