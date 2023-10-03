using Doppler.ImageAnalyzer.Api.Services.Repositories.Entities;
using Doppler.ImageAnalyzer.Api.Services.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddMongoDBRepositoryService(this IServiceCollection services, IConfiguration configuration)
        {
            var repositorySettingsSection = configuration.GetSection(nameof(RepositorySettings));

            services.Configure<RepositorySettings>(repositorySettingsSection);

            var repositorySettings = new RepositorySettings();
            repositorySettingsSection.Bind(repositorySettings);

            var mongoUrlBuilder = new MongoUrlBuilder(repositorySettings.ConnectionString)
            {
                DatabaseName = repositorySettings.DatabaseName,
                Password = repositorySettings.Password,
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

            services.AddSingleton<IImageAnalysisResultRepository, ImageAnalysisResultMongoDBRepository>();

            return services;
        }
    }
}
