using MongoDB.Bson;
using MongoDB.Driver;

namespace CoreUtilities.Data.Connections
{
    public interface IBulkDatabaseWriter
    {
        Task AddDeleteModel(ObjectId id);
        Task AddInsertModel(BsonDocument document);
        Task AddReplaceModel(BsonDocument document);
        Task AddUpdateModel(ObjectId id, UpdateDefinition<BsonDocument> updateDefinition);
        Task BulkWriteAsync();
        void ChangeBatchSize(int batchSize);
        void ChangeCollectionName(string collectionName);
    }
}