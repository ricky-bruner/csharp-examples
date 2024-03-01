using IntervalProcessing.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Interfaces
{
    public interface IWriterFactory
    {
        IWriter<BsonDocument> CreateWriter(string key, FileInfo fileInfo);
    }
}
