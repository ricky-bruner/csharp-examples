using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class GeneratedFile
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; }

        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; }

        [BsonElement("fileSize")]
        public string FileSize { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }
    }
}
