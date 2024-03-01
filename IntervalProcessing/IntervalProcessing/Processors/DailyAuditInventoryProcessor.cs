using IntervalProcessing.Configurations;
using IntervalProcessing.Data.Connections;
using IntervalProcessing.Data.Managers;
using IntervalProcessing.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class DailyAuditInventoryProcessor : BaseFileGenerationProcessor
    {
        public DailyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager)
            : base (connection, config, fileProcessorConfigManager, typeof(DailyAuditInventoryProcessor), writerFactory, queryManager)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            return document;
        }
    }
}
