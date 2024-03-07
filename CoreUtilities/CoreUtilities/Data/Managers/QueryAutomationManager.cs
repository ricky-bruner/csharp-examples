using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Enums;
using CoreUtilities.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using static CoreUtilities.Constants.DatabaseCollections;

namespace CoreUtilities.Data.Managers
{
    public class QueryAutomationManager : IQueryAutomationManager
    {
        private IMongoConnection<BsonDocument> _connection;

        public QueryAutomationManager(IMongoConnection<BsonDocument> connection)
        {
            _connection = connection;
        }

        public async Task<List<DataUpdateConfig>> GetQueryBasedDataUpdateConfigsAsync(RunCadence runCadence)
        {
            _connection.SetCollection(DataUpdateConfigs);

            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("runCadence", runCadence.ToString());

            List<BsonDocument> results = await _connection.Collection.Find(filter).ToListAsync();

            try
            {
                List<DataUpdateConfig> queryBasedDataUpdateConfigs = results
                    .Select(bsonDoc => BsonSerializer.Deserialize<DataUpdateConfig>(bsonDoc))
                    .ToList();
                return queryBasedDataUpdateConfigs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing DataUpdateConfig in {this.GetType()}" + Environment.NewLine + ex);
                throw;
            }
        }
    }
}