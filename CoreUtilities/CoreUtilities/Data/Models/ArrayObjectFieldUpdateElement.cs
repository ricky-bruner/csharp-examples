using CoreUtilities.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class ArrayObjectFieldUpdateElement
    {
        [BsonElement("arrayProperty")]
        public string ArrayProperty { get; set; }

        [BsonElement("matchingPropertyName")]
        public string MatchingPropertyName { get; set; }

        [BsonElement("updateType")]
        public UpdateType UpdateType { get; set; }

        [BsonElement("newValue")]
        public string NewValue { get; set; }
    }
}
