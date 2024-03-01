using IntervalProcessing.Configurations;
using IntervalProcessing.Data.Connections;
using IntervalProcessing.Data.Managers;
using IntervalProcessing.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class WeeklyAuditInventoryProcessor : BaseFileGenerationProcessor
    {
        public WeeklyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager)
            : base(connection, config, fileProcessorConfigManager, typeof(WeeklyAuditInventoryProcessor), writerFactory, queryManager)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

            customizedDoc.Add("resourceCode", "RCP-1039bc0");
            customizedDoc.Add("externalId", "8b10YTG");
            
            return customizedDoc;
        }
    }
}