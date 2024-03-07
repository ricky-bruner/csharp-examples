using CoreUtilities.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class SingleFieldUpdateElement
    {
        [BsonElement("elementName")]
        public string ElementName { get; set; }

        [BsonElement("updateType")]
        public UpdateType UpdateType { get; set; }

        [BsonElement("elementValue")]
        public string ElementValue { get; set; }

    }
}
