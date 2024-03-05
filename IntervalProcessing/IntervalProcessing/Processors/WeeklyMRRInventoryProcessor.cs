using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;
using System.Text;

namespace IntervalProcessing.Processors
{
    public class WeeklyMRRInventoryProcessor : BaseFileGenerationProcessor
    {
        private string _del = "~";

        public WeeklyMRRInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager)
            : base(connection, config, fileProcessorConfigManager, typeof(WeeklyMRRInventoryProcessor), writerFactory, queryManager)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

            StringBuilder sb = new StringBuilder();
            sb.Append(document["provider"]["address"]["address1"].AsString.Trim());
            sb.Append(_del);
            sb.Append(document["provider"]["address"]["address2"].AsString.Trim());
            sb.Append(_del);
            sb.Append(document["provider"]["address"]["city"].AsString.Trim());
            sb.Append(",");
            sb.Append(_del);
            sb.Append(document["provider"]["address"]["state"].AsString.Trim());
            sb.Append(_del);
            sb.Append(document["provider"]["address"]["zip"].AsString.Trim());
            sb.Replace(_del, " ");

            customizedDoc.Add("builtProviderAddress", sb.ToString());

            return customizedDoc;
        }
    }
}