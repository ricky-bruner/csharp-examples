using IntervalProcessing.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace IntervalProcessing.Configurations
{
    [BsonIgnoreExtraElements]
    public class FileProcessorConfig
    {
        [BsonElement("processes")]
        public List<FileProcessorSetting> Processes { get; }
    }
}
