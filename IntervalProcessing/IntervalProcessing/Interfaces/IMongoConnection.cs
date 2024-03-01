using MongoDB.Driver;

namespace IntervalProcessing.Interfaces
{
    public interface IMongoConnection<T>
    {
        IMongoCollection<T> Collection { get; }
        void SetCollection(string collectionName);

    }
}
