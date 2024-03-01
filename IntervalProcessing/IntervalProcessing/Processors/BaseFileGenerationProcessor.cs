using IntervalProcessing.Configurations;
using IntervalProcessing.Data.Connections;
using IntervalProcessing.Data.Managers;
using IntervalProcessing.Data.Models;
using IntervalProcessing.Writers;
using MongoDB.Bson;

namespace IntervalProcessing.Processors
{
    public abstract class BaseFileGenerationProcessor : IFileProcessor
    {
        private IMongoConnection<BsonDocument> _connection { get; set; }
        private IConfig _config { get; set; }
        private FileProcessorSpecification _settings { get; set; }
        private readonly IWriterFactory _writerFactory;
        private IWriter<BsonDocument> _fileWriter { get; set; }
        private IStoredQueryManager _queryManager { get; set; }

        public BaseFileGenerationProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, Type processorType, IWriterFactory writerFactory, IStoredQueryManager queryManager) 
        { 
            _writerFactory = writerFactory;
            _connection = connection;
            _config = config;
            _queryManager = queryManager;
            _settings = fileProcessorConfigManager.GetFileProcessorSpecification(processorType).Result;
        }

        public void Execute() 
        { 
            string query = _queryManager.GetQueryAsync(_settings.QueryName).Result;

            FileInfo workingFile = new FileInfo($"{_config.WorkingDirectory.FullName}{Path.DirectorySeparatorChar}{_settings.FileNameBase}_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.txt");

            _fileWriter = _writerFactory.CreateWriter(_settings.WriterTypeKey, workingFile);

            MongoCursor cursor = new MongoCursor(query, _settings.Projection, _settings.CollectionName, _connection);

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
