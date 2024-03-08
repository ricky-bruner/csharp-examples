using MongoDB.Bson.Serialization.Attributes;

namespace CoreUtilities.Data.Models
{
    [BsonIgnoreExtraElements]
    public class AWSSettings
    {
        [BsonElement("accessKey")]
        public string AccessKey { get; set; }

        [BsonElement("secretKey")]
        public string SecretKey { get; set; }

        [BsonElement("region")] 
        public string Region { get; set; }

        [BsonElement("rootBucket")] 
        public string RootBucket { get; set; }
    }
}
