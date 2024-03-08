﻿using CoreUtilities.CloudServices.AWS;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public class WeeklyAuditInventoryProcessor : BaseFileGenerationProcessor
    {
        public WeeklyAuditInventoryProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, IWriterFactory writerFactory, IStoredQueryManager queryManager, IS3Uploader s3Uploader)
            : base(connection, config, fileProcessorConfigManager, typeof(WeeklyAuditInventoryProcessor), writerFactory, queryManager, s3Uploader)
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