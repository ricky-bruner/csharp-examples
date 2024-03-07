using CoreUtilities.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class DataUpdateConfig
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("query")]
        public string Query { get; set; }

        [BsonElement("collectionName")]
        public string CollectionName { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("singleFieldUpdateElements")]
        public List<SingleFieldUpdateElement> SingleFieldUpdateElements { get; set; }

        [BsonElement("arrayObjectFieldUpdateElements")]
        public List<ArrayObjectFieldUpdateElement> ArrayObjectFieldUpdateElements { get; set; }

        [BsonElement("runCadence")]
        public RunCadence RunCadence { get; set; }
    }
}
