using MongoDB.Bson.Serialization.Attributes;

namespace IntervalProcessing.Configurations
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class FileProcessorSetting
    {
        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("configurations")]
        public FileProcessorSpecification? Configurations { get; set; }
    }
}
