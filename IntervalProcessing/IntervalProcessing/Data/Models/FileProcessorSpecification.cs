using MongoDB.Bson.Serialization.Attributes;

namespace IntervalProcessing.Data.Models
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class FileProcessorSpecification
    {
        [BsonElement("queryName")]
        public string QueryName { get; set; }

        [BsonElement("collectionName")]
        public string CollectionName { get; set; }

        [BsonElement("projection")]
        public string Projection { get; set; }

        [BsonElement("fileNameBase")]
        public string FileNameBase { get; set; }

        [BsonElement("writerTypeKey")]
        public string WriterTypeKey { get; set; }
    }
}
