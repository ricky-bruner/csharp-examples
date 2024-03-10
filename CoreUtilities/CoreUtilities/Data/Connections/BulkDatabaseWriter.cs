using MongoDB.Bson;
using MongoDB.Driver;

namespace CoreUtilities.Data.Connections
{
    public class BulkDatabaseWriter
    {
        private IMongoConnection<BsonDocument> _connection {  get; set; }
        private List<WriteModel<BsonDocument>> _bulkWriteList { get; set; }
        private int _batchSize { get; set; }
        private string _collectionName { get; set; }

        public BulkDatabaseWriter(IMongoConnection<BsonDocument> connection) 
        { 
            _connection = connection;
            _bulkWriteList = new List<WriteModel<BsonDocument>>();
            _batchSize = 10000;
        }

        public async Task BulkWriteAsync() 
        {
            if (_bulkWriteList.Count > 0) 
            {
                _connection.SetCollection(_collectionName);
                await _connection.Collection.BulkWriteAsync(_bulkWriteList);
                _bulkWriteList.Clear();
            }
        }

        private async Task AddWriteModel(WriteModel<BsonDocument> model) 
        { 
            _bulkWriteList.Add(model);

            if (_bulkWriteList.Count >= _batchSize) 
            { 
                await BulkWriteAsync();
            }
        }

        public async Task AddInsertModel(BsonDocument document) =>
            await AddWriteModel(new InsertOneModel<BsonDocument>(document));

        public async Task AddUpdateModel(ObjectId id, UpdateDefinition<BsonDocument> updateDefinition) =>
            await AddWriteModel(new UpdateOneModel<BsonDocument>(Builders<BsonDocument>.Filter.Eq("_id", id), updateDefinition));

        public async Task AddReplaceModel(BsonDocument document) =>
            await AddWriteModel(new ReplaceOneModel<BsonDocument>(Builders<BsonDocument>.Filter.Eq("_id", document["_id"]), document));

        public async Task AddDeleteModel(ObjectId id) =>
            await AddWriteModel(new DeleteOneModel<BsonDocument>(Builders<BsonDocument>.Filter.Eq("_id", id)));

        public void ChangeBatchSize(int batchSize) => _batchSize = batchSize;

        public void ChangeCollectionName(string collectionName) 
        { 
            _collectionName = collectionName;
            _connection.SetCollection(collectionName);
        }
    }
}
