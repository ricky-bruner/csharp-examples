﻿using CoreUtilities.CloudServices.AWS;
using CoreUtilities.CloudServices.Utilities;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.ExtensionMethods;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;
using System.Text;

namespace IntervalProcessing.Processors
{
    public class WeeklyMRRInventoryProcessor : BaseFileGenerationProcessor
    {
        private string _del = "~";

        public WeeklyMRRInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager, IGeneratedFileUploader uploader)
            : base(connection, config, fileProcessorConfigManager, typeof(WeeklyMRRInventoryProcessor), writerFactory, queryManager, uploader)
        {

        }

        public override BsonDocument ApplyCustomLogic(BsonDocument document)
        {
            BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

            StringBuilder sb = new StringBuilder();
            sb.Append(document.GetStringValue("provider.address.address1").Trim());
            sb.Append(_del);
            sb.Append(document.GetStringValue("provider.address.address2").Trim());
            sb.Append(_del);
            sb.Append(document.GetStringValue("provider.address.city").Trim());
            sb.Append(",");
            sb.Append(_del);
            sb.Append(document.GetStringValue("provider.address.state").Trim());
            sb.Append(_del);
            sb.Append(document.GetStringValue("provider.address.zip").Trim());
            sb.Replace(_del, " ");

            customizedDoc.Add("builtProviderAddress", sb.ToString());

            return customizedDoc;
        }
    }
}