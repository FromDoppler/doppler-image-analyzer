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

            var mongoUrlBuilder = new MongoUrlBuilder(repositorySettings.ConnectionString);
            mongoUrlBuilder.DatabaseName ??= repositorySettings.DefaultDatabaseName;
            mongoUrlBuilder.Password ??= repositorySettings.SecretPassword;

            var mongoUrl = mongoUrlBuilder.ToMongoUrl();
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            services.AddSingleton<IMongoClient>(x =>
            {
                #region ImageAnalysisResult_Indexes

                var imageAnalysisResult_Collection = database.GetCollection<BsonDocument>(ImageAnalysisResultDocumentInfo.CollectionName);

                var imageAnalysisResult_ImagesCount_Index = new CreateIndexModel<BsonDocument>(
                    Builders<BsonDocument>.IndexKeys.Ascending(ImageAnalysisResultDocumentInfo.ImagesCount_PropName)
                );
                imageAnalysisResult_Collection.Indexes.CreateOne(imageAnalysisResult_ImagesCount_Index);

                #endregion

                return mongoClient;
            });
            services.AddSingleton(x => mongoClient.GetDatabase(mongoUrl.DatabaseName));

            services.AddSingleton<IImageAnalysisResultRepository, ImageAnalysisResultMongoDBRepository>();

            return services;
        }
    }
}
