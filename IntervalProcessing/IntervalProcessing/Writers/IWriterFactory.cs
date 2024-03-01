using MongoDB.Bson;

namespace IntervalProcessing.Writers
{
    public interface IWriterFactory
    {
        IWriter<BsonDocument> CreateWriter(string key, FileInfo fileInfo);
    }
}
