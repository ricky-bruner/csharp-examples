using CoreUtilities.CloudServices.AWS;
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
        public DailyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager, IS3Uploader s3Uploader)
            : base (connection, config, fileProcessorConfigManager, typeof(DailyAuditInventoryProcessor), writerFactory, queryManager, s3Uploader)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            return document;
        }
    }
}
