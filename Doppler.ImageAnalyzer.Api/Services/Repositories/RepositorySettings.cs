using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories
{
    public class RepositorySettings
    {
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Default database name to use when is not specified in ConnectionUrl
        /// </summary>
        public string DefaultDatabaseName => "imageanalyzer_db";

        /// <summary>
        /// Secret password to use when is not specified in ConnectionUrl
        /// </summary>
        public string SecretPassword { get; set; }
    }
}
