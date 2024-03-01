using MongoDB.Driver;

namespace IntervalProcessing.Data.Connections
{
    public interface IMongoConnection<T>
    {
        IMongoCollection<T> Collection { get; }
        void SetCollection(string collectionName);

    }
}
