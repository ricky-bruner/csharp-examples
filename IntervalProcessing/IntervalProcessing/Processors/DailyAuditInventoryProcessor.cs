using IntervalProcessing.Interfaces;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class DailyAuditInventoryProcessor : BaseFileGenerationProcessor
    {
        private string _processingKey { get; set; }

        public DailyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IFileProcessorOptions options, IWriterFactory writerFactory, IStoredQueryManager queryManager)
            : base (connection, options, writerFactory, queryManager)
        {
            _processingKey = "DailyAuditInventoryProcessor";
        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            return document;
        }
    }
}
