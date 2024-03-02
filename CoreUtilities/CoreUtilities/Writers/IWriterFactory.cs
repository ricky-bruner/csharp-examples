using MongoDB.Bson;

namespace CoreUtilities.Writers
{
    public interface IWriterFactory
    {
        IWriter<BsonDocument> CreateWriter(string key, FileInfo fileInfo);
    }
}
