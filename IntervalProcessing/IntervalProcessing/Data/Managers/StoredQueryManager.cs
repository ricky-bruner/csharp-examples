using IntervalProcessing.Data.Connections;
using MongoDB.Bson;
using MongoDB.Driver;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing.Data.Managers
{
    public class StoredQueryManager : IStoredQueryManager
    {
        private IMongoConnection<BsonDocument> _connection;

        public StoredQueryManager(IMongoConnection<BsonDocument> connection)
        {
            _connection = connection;
        }

        public async Task<string> GetQueryAsync(string queryName)
        {
            _connection.SetCollection(StoredQueries);

            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", queryName);

            BsonDocument result = await _connection.Collection.Find(filter).FirstOrDefaultAsync();

            return result != null ? result["query"].AsString : string.Empty;
        }
    }
}
