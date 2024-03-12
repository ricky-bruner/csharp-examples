using CoreUtilities.CloudServices.AWS;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using static CoreUtilities.Constants.DatabaseCollections;

namespace CoreUtilities.CloudServices.Utilities
{
    public class GeneratedFileUploader : IGeneratedFileUploader
    {
        private IMongoConnection<BsonDocument> _connection { get; set; }
        private IS3Uploader _s3Uploader { get; set; }

        public GeneratedFileUploader(IMongoConnection<BsonDocument> connection, IS3Uploader s3Uploader)
        {
            _connection = connection;
            _s3Uploader = s3Uploader;
        }

        public async Task Upload(FileInfo file, string s3Location, string source)
        {
            GeneratedFile completedFile = new GeneratedFile(file, await _s3Uploader.GetBucketAsync(), s3Location, source);

            //future logic for sftp delivery
            await _s3Uploader.UploadFileAsync(file.FullName, completedFile);

            completedFile.Bucket = await _s3Uploader.GetBucketAsync();

            string startCollection = _connection.Collection.CollectionNamespace.CollectionName;

            _connection.SetCollection(GeneratedFiles);

            await _connection.Collection.InsertOneAsync(completedFile.ToBsonDocument());

            _connection.SetCollection(startCollection);
        }

        public async Task Download(FileInfo file, ObjectId id)
        {
            string startCollection = _connection.Collection.CollectionNamespace.CollectionName;

            _connection.SetCollection(GeneratedFiles);

            BsonDocument bsonGeneratedFile = await _connection.Collection.Find(Builders<BsonDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
            GeneratedFile generatedFile = BsonSerializer.Deserialize<GeneratedFile>(bsonGeneratedFile);

            await _s3Uploader.DownloadFileAsync(file.FullName, generatedFile.Location);

            _connection.SetCollection(startCollection);
        }
    }
}
