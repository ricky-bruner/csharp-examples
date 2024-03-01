using IntervalProcessing.Data.Connections;
using IntervalProcessing.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing.Data.Managers
{
    public class FileProcessorConfigManager : IFileProcessorConfigManager
    {
        private IMongoConnection<BsonDocument> _connection;

        public Dictionary<string, FileProcessorSpecification> Settings { get; private set; }

        public FileProcessorConfigManager(IMongoConnection<BsonDocument> connection)
        {
            _connection = connection;
        }

        private async Task<Dictionary<string, FileProcessorSpecification>> GetSettingsAsync()
        {
            if (Settings == null)
            {
                _connection.SetCollection(IntervalProcessingConfigs);

                BsonDocument result = (await _connection.Collection.FindAsync(BsonSerializer.Deserialize<BsonDocument>("{}"))).FirstOrDefault();

                try
                {
                    FileProcessorConfig allSettings = BsonSerializer.Deserialize<FileProcessorConfig>(result);

                    return Settings = allSettings.Processes
                        .Where(setting => setting.Name != null && setting.Configurations != null)
                        .GroupBy(setting => setting.Name!)
                        .ToDictionary(
                            group => group.Key,
                            group => group.First().Configurations!
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deserializing FileProcessorConfig " + ex.ToString());
                    throw;
                }
            }
            else
            {
                return Settings;
            }
        }

        public async Task<FileProcessorSpecification> GetFileProcessorSpecification(Type processorType)
        {
            if (Settings == null)
            {
                await GetSettingsAsync();
            }

            return Settings.ContainsKey(processorType.Name) ? Settings[processorType.Name] : throw new KeyNotFoundException();
        }
    }
}
