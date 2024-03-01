using IntervalProcessing.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace IntervalProcessing.Utilities
{
    public class MongoCursor
    {
        public string query { get; set; }

        public string projection { get; set; }

        public string? sort { get; set; }

        private string _collectionName;

        private IMongoConnection<BsonDocument> _connection;

        public MongoCursor(string query, string projection, string collectionName, IMongoConnection<BsonDocument> connection)
        { 
            this.query = query;
            this.projection = projection;
            sort = null;
            _collectionName = collectionName;
            _connection = connection;
            _connection.SetCollection(collectionName);
        }

        public MongoCursor(string query, string projection, string sort, string collectionName, IMongoConnection<BsonDocument> connection)
        {
            this.query = query;
            this.projection = projection;
            this.sort = sort;
            _collectionName = collectionName;
            _connection = connection;
            _connection.SetCollection(collectionName);
        }

        public async Task<bool> ExecuteCursor(Action<BsonDocument> delegatedAction, int? batchSize = null) 
        {
            FindOptions<BsonDocument> options = SetupCursorOptions(batchSize);

            using (IAsyncCursor<BsonDocument> cursor = await _connection.Collection.FindAsync(BsonSerializer.Deserialize<BsonDocument>(this.query), options))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;

                    foreach (BsonDocument document in batch)
                    {
                        delegatedAction(document);
                    }
                }
            }

            return true;
        }

        private FindOptions<BsonDocument> SetupCursorOptions(int? batchSize) 
        {
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                Projection = BsonSerializer.Deserialize<BsonDocument>(this.projection),
                NoCursorTimeout = false, 
            };

            if (batchSize != null)
            {
                options.BatchSize = batchSize;
            }

            if (!string.IsNullOrEmpty(this.sort))
            {
                options.Sort = BsonSerializer.Deserialize<BsonDocument>(this.sort);
            }

            return options;
        }

        public void ChangeCollection(string collectionName) 
        {
            _collectionName = collectionName;
            _connection.SetCollection(_collectionName);
        }
    }
}
