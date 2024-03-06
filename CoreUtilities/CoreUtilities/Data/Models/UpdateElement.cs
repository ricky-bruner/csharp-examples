using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class UpdateElement
    {
        [BsonElement("elementName")]
        public string ElementName { get; set; }

        [BsonElement("updateType")]
        public string UpdateType { get; set; }

        [BsonElement("elementValue")]
        public string ElementValue { get; set; }

    }
}
