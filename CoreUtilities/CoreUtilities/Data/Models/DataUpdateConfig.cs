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

        [BsonElement("updateElements")]
        public List<UpdateElement> UpdateElements { get; set; }

        [BsonElement("runCadence")]
        public RunCadence RunCadence { get; set; }
    }
}
