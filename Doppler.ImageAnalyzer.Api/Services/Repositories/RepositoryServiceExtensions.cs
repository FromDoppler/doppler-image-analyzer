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

            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(x =>
            {
                var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
                CreateMongoDBIndexes(database);
                return database;
            });

            services.AddSingleton<IImageAnalysisResultRepository, ImageAnalysisResultMongoDBRepository>();

            return services;
        }

        private static void CreateMongoDBIndexes(IMongoDatabase database)
        {
            var imageAnalysisResult_Collection = database.GetCollection<BsonDocument>(ImageAnalysisResultDocumentInfo.CollectionName);

            var imageAnalysisResult_ImagesCount_Index = new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending(ImageAnalysisResultDocumentInfo.ImagesCount_PropName)
            );
            imageAnalysisResult_Collection.Indexes.CreateOne(imageAnalysisResult_ImagesCount_Index);
        }
    }
}
