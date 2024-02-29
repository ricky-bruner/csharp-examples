using IntervalProcessing.Utilities;
using IntervalProcessing.Writers;
using MongoDB.Bson;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing.Processors
{
    public class InventoryFileProcessor
    {
        private MongoConnection<BsonDocument> _connection { get; set; }

        private string queryName { get; set; }

        private string projection { get; set; }

        private string _fileNameBase { get; set; }

        private Type _writerType { get; set; }

        private BaseWriter<BsonDocument>? _fileWriter { get; set; }

        private StoredQueryManager _queryManager { get; set; }

        public InventoryFileProcessor(MongoConnection<BsonDocument> connection, string queryName, string projection, string fileNameBase, Type writerType) 
        { 
            this.queryName = queryName;
            this.projection = projection;
            _fileNameBase = fileNameBase;
            _writerType = writerType;
            _connection = connection;
            _queryManager = new StoredQueryManager(_connection);
        }

        public void Execute() 
        { 
            string query = _queryManager.GetQueryAsync(queryName).Result;

            FileInfo inventoryFile = new FileInfo($"{Config.GetWorkingDirectory().FullName}{Path.DirectorySeparatorChar}{_fileNameBase}_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.txt");

            _fileWriter = (BaseWriter<BsonDocument>?)Activator.CreateInstance(_writerType, inventoryFile);

            MongoCursor cursor = new MongoCursor(query, projection, Audits, _connection);

            cursor.ExecuteCursor(WriteDataToFile).Wait();

            _fileWriter.Close();
            _fileWriter.Dispose();

            //future logic for s3 upload and sftp delivery
        }

        private void WriteDataToFile(BsonDocument document) 
        { 
            _fileWriter.Write(document);
        }
    }
}
