using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using static CoreUtilities.Constants.DatabaseCollections;

namespace CoreUtilities.Data.Managers
{
    public class AWSSettingsManager : IAWSSettingsManager
    {
        private IMongoConnection<BsonDocument> _connection;

        public AWSSettingsManager(IMongoConnection<BsonDocument> connection)
        {
            _connection = connection;
        }

        public async Task<AWSSettings> GetAWSSettingsAsync()
        {
            _connection.SetCollection(AWSSettingsCollection);

            BsonDocument results = await _connection.Collection.Find(BsonSerializer.Deserialize<BsonDocument>("{}")).FirstOrDefaultAsync();

            try
            {
                return BsonSerializer.Deserialize<AWSSettings>(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing AWSSettings in {this.GetType()}" + Environment.NewLine + ex);
                throw;
            }
        }
    }
}