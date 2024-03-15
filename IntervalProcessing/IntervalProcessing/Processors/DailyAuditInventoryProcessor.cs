using CoreUtilities.CloudServices.AWS;
using CoreUtilities.CloudServices.Utilities;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class DailyAuditInventoryProcessor : BaseFileGenerationProcessor
    {
        public DailyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager, IGeneratedFileUploader uploader)
            : base (connection, config, fileProcessorConfigManager, typeof(DailyAuditInventoryProcessor), writerFactory, queryManager, uploader)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            return document;
        }
    }
}
