using IntervalProcessing.Interfaces;
using IntervalProcessing.Utilities;
using MongoDB.Bson;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing.Processors
{
    public abstract class BaseFileGenerationProcessor : IFileProcessor
    {
        private IMongoConnection<BsonDocument> _connection { get; set; }

        private string _queryName { get; set; }

        private string _collectionName { get; set; }

        private string _projection { get; set; }

        private string _fileNameBase { get; set; }

        private string _writerTypeKey { get; set; }


        private readonly IWriterFactory _writerFactory;

        private IWriter<BsonDocument>? _fileWriter { get; set; }

        private IStoredQueryManager _queryManager { get; set; }

        public BaseFileGenerationProcessor(IMongoConnection<BsonDocument> connection, IFileProcessorOptions options, IWriterFactory writerFactory, IStoredQueryManager queryManager) 
        { 
            _queryName = options.QueryName;
            _collectionName = options.CollectionName;
            _projection = options.Projection;
            _fileNameBase = options.FileNameBase;
            _writerTypeKey = options.WriterTypeKey;
            _writerFactory = writerFactory;
            _connection = connection;
            _queryManager = queryManager;
        }

        public void Execute() 
        { 
            string query = _queryManager.GetQueryAsync(_queryName).Result;

            FileInfo inventoryFile = new FileInfo($"{CoreConfig.WorkingDirectory.FullName}{Path.DirectorySeparatorChar}{_fileNameBase}_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.txt");

            _fileWriter = _writerFactory.CreateWriter(_writerTypeKey, inventoryFile);

            MongoCursor cursor = new MongoCursor(query, _projection, Audits, _connection);

            cursor.ExecuteCursor(WriteDataToFile).Wait();

            _fileWriter.Close();

            //future logic for s3 upload and sftp delivery
        }

        private void WriteDataToFile(BsonDocument document) 
        { 
            _fileWriter.Write(ApplyCustomLogic(document));
        }

        public abstract BsonDocument ApplyCustomLogic(BsonDocument document);
    }
}
