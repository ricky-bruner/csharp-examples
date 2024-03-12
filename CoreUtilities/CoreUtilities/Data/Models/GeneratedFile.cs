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

        [BsonElement("bucket")]
        public string Bucket { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("source")]
        public string Source { get; set; }

        public GeneratedFile(FileInfo file, string bucket, string s3Location, string creationSource)
        {
            Id = ObjectId.GenerateNewId();
            FileName = file.Name;
            DateCreated = file.CreationTime;
            FileSize = file.Length.ToString();
            Bucket = bucket;
            Location = $"{s3Location}/{Id}{Path.GetExtension(file.Name)}";
            Source = creationSource;
        }
    }
}
