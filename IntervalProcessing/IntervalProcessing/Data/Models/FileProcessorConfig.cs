using MongoDB.Bson.Serialization.Attributes;

namespace IntervalProcessing.Data.Models
{
    [BsonIgnoreExtraElements]
    public class FileProcessorConfig
    {
        [BsonElement("processes")]
        public List<FileProcessorSetting> Processes { get; set; }
    }
}
