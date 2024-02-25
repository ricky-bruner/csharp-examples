using MongoDB.Driver;

namespace IntervalProcessing.Utilities
{
    public class MongoConnection<T>
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        public IMongoCollection<T> Collection { get; private set; }

        // Constructor that initializes the MongoClient, and stores the database and collection.
        public MongoConnection(string connectionString, string databaseName, string collectionName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            Collection = _database.GetCollection<T>(collectionName);
        }
    }
}
