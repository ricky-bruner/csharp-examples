using MongoDB.Driver;

namespace CoreUtilities.Data.Connections
{
    public interface IMongoConnection<T>
    {
        IMongoCollection<T> Collection { get; }
        void SetCollection(string collectionName);

    }
}
