using MongoDB.Driver;

namespace Doppler.ImageAnalyzer.Api.Services.Repositories
{
    public class ImageAnalyzerMongoDBContextSettings
    {
        public string? ConnectionString { get; set; }

        private string? _databaseName;
        public string? DatabaseName
        {
            get
            {
                var mongoUrl = new MongoUrl(ConnectionString);
                return mongoUrl.DatabaseName ?? _databaseName;
            }
            set
            {
                _databaseName = value;
            }
        }

        private string? _password;
        public string? Password
        {
            get
            {
                var mongoUrl = new MongoUrl(ConnectionString);
                return mongoUrl.Password ?? _password;
            }
            set
            {
                _password = value;
            }
        }
    }
}
