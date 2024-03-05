using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class DailyActiveMRRInventoryProcessor : BaseFileGenerationProcessor
    {
        public DailyActiveMRRInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager)
            : base(connection, config, fileProcessorConfigManager, typeof(DailyActiveMRRInventoryProcessor), writerFactory, queryManager)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

            if (customizedDoc["organization"].AsString == "Transparency Health") 
            {
                customizedDoc["organization"] = "Clear View Health Services";
            }

            return customizedDoc;
        }
    }
}