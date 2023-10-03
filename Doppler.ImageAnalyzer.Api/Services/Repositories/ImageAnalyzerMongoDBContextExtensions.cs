using Doppler.ImageAnalyzer.Api.Services.Respositories.Entities;
using Doppler.ImageAnalyzer.Api.Services.Respositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.Respositories
{
    public static class ImageAnalyzerMongoDBContextExtensions
    {
        public static IServiceCollection AddImageAnalyzerMongoDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDBContextSettingsSection = configuration.GetSection(nameof(ImageAnalyzerMongoDBContextSettings));

            services.Configure<ImageAnalyzerMongoDBContextSettings>(mongoDBContextSettingsSection);

            var imageAnalizerMongoDBContextSettings = new ImageAnalyzerMongoDBContextSettings();
            mongoDBContextSettingsSection.Bind(imageAnalizerMongoDBContextSettings);

            var mongoUrlBuilder = new MongoUrlBuilder(imageAnalizerMongoDBContextSettings.ConnectionString)
            {
                DatabaseName = imageAnalizerMongoDBContextSettings.DatabaseName,
                Password = imageAnalizerMongoDBContextSettings.Password,
            };

            var mongoUrl = mongoUrlBuilder.ToMongoUrl();

            services.AddSingleton<IMongoClient>(x =>
            {
                var mongoClient = new MongoClient(mongoUrl);
                var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

                #region ImageAnalysisResult_Indexes

                var imageAnalysisResult_Collection = database.GetCollection<BsonDocument>(ImageAnalysisResultDocumentInfo.CollectionName);

                var imageAnalysisResult_StatusCode_Index = new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Ascending(ImageAnalysisResultDocumentInfo.StatusCode_PropName)
                );
                imageAnalysisResult_Collection.Indexes.CreateOne(imageAnalysisResult_StatusCode_Index);

                var imageAnalysisResult_ImagesCount_Index = new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Ascending(ImageAnalysisResultDocumentInfo.ImagesCount_PropName)
                );
                imageAnalysisResult_Collection.Indexes.CreateOne(imageAnalysisResult_ImagesCount_Index);

                #endregion

                return mongoClient;
            });

            services.AddSingleton<IImageAnalysisResultService, ImageAnalysisResultService>();

            return services;
        }
    }
}
