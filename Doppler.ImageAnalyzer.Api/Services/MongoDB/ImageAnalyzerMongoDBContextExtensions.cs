using Doppler.PushContact.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.MongoDB
{
    public static class ImageAnalyzerMongoDBContextExtensions
    {
        public static IServiceCollection AddImageAnalyzerMongoDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDBContextSettingsSection = configuration.GetSection(nameof(ImageAnalyzerMongoDBContextSettings));

            services.Configure<ImageAnalyzerMongoDBContextSettings>(mongoDBContextSettingsSection);

            var imageAnalizerMongoDBContextSettings = new ImageAnalyzerMongoDBContextSettings();
            mongoDBContextSettingsSection.Bind(imageAnalizerMongoDBContextSettings);

            var mongoUrlBuilder = new MongoUrlBuilder(imageAnalizerMongoDBContextSettings.ConnectionString);

            // when these values were not specified in the "ConnectionString"
            mongoUrlBuilder.DatabaseName ??= imageAnalizerMongoDBContextSettings.DatabaseName;
            mongoUrlBuilder.Password ??= imageAnalizerMongoDBContextSettings.Password;

            var mongoUrl = mongoUrlBuilder.ToMongoUrl();

            services.AddSingleton<IMongoClient>(x =>
            {
                var mongoClient = new MongoClient(mongoUrl);
                var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

                // TODO: create indexes

                return mongoClient;
            });

            return services;
        }
    }
}
