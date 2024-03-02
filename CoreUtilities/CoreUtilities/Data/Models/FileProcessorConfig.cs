using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class FileProcessorConfig
    {
        [BsonElement("processes")]
        public List<FileProcessorSetting> Processes { get; set; }
    }
}
