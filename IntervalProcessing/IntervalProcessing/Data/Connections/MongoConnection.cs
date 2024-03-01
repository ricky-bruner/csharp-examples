using IntervalProcessing.Configurations;
using MongoDB.Driver;

namespace IntervalProcessing.Data.Connections
{
    public class MongoConnection<T> : IMongoConnection<T>
    {
        private readonly string _connectionString;

        private string _databaseName;

        private string _collectionName;

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<T> Collection { get; set; }

        public string ConnectionString => _connectionString;

        public MongoConnection(IConfig config, string collectionName)
        {
            _connectionString = config.MongoConnectionString;
            _databaseName = config.DatabaseName;
            _collectionName = collectionName;

            if (config.IsServerlessDB)
            {
                Client = new MongoClient(GetClientSettings(_connectionString));
            }
            else
            {
                Client = new MongoClient(_connectionString);
            }

            Database = Client.GetDatabase(_databaseName);
            Collection = Database.GetCollection<T>(_connectionString);
        }

        public void SetDatabase(string databaseName)
        {
            _databaseName = databaseName;
            Database = Client.GetDatabase(_databaseName);
        }

        public void SetCollection(string collectionName)
        {
            _collectionName = collectionName;
            Collection = Database.GetCollection<T>(_collectionName);
        }

        private MongoClientSettings GetClientSettings(string connectionString)
        {
            MongoUrl url = new MongoUrl(connectionString);
            MongoClientSettings clientSettings = MongoClientSettings.FromUrl(url);

            clientSettings.SslSettings = new SslSettings
            {
                CheckCertificateRevocation = false
            };

            clientSettings.UseSsl = true;
            clientSettings.VerifySslCertificate = false;

            return clientSettings;
        }
    }
}
