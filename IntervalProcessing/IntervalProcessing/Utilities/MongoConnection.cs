using MongoDB.Driver;

namespace IntervalProcessing.Utilities
{
    public class MongoConnection<T>
    {
        private readonly string _connectionString;

        private string _databaseName;

        private string _collectionName;

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<T> Collection { get; set; }

        public string ConnectionString => _connectionString;

        public MongoConnection(string connectionString, string databaseName, string collectionName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            _collectionName = collectionName;

            if (Config.GetIsServerless())
            {
                Client = new MongoClient(GetClientSettings(_connectionString));
            }
            else
            {
                Client = new MongoClient(_connectionString);
            }

            Database = Client.GetDatabase(_connectionString);
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
