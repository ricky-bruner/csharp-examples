using CoreUtilities.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Writers
{
    public class WriterFactory : IWriterFactory
    {
        public WriterFactory() { }

        public IWriter<BsonDocument> CreateWriter(string key, FileInfo fileInfo) 
        {
            switch (key)
            {
                case "DailyAuditInventoryFileWriter":
                    return new DailyAuditInventoryFileWriter<BsonDocument>(fileInfo);

                case "WeeklyAuditInventoryFileWriter":
                    return new WeeklyAuditInventoryFileWriter<BsonDocument>(fileInfo);

                case "DailyActiveMRRInventoryFileWriter":
                    return new DailyActiveMRRInventoryFileWriter<BsonDocument>(fileInfo);

                case "WeeklyMRRInventoryFileWriter":
                    return new WeeklyMRRInventoryFileWriter<BsonDocument>(fileInfo);

                default:
                    return null;
            }
        }
    }
}
