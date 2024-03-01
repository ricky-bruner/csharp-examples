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

                default:
                    return null;
            }
        }
    }
}
