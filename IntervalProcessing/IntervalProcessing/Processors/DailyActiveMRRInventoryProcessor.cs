using CoreUtilities.CloudServices.AWS;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.ExtensionMethods;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class DailyActiveMRRInventoryProcessor : BaseFileGenerationProcessor
    {
        public DailyActiveMRRInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager, IS3Uploader s3Uploader)
            : base(connection, config, fileProcessorConfigManager, typeof(DailyActiveMRRInventoryProcessor), writerFactory, queryManager, s3Uploader)
        {

        }

        public override async Task Execute() { }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

            if (customizedDoc.GetStringValue("organization") == "Transparency Health") 
            {
                customizedDoc["organization"] = "Clear View Health Services";
            }

            return customizedDoc;
        }
    }
}