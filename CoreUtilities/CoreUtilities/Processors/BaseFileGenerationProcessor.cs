using CoreUtilities.Writers;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace CoreUtilities.Processors
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

        public async void Execute()
        {
            string query = _queryManager.GetQueryAsync(_settings.QueryName).Result;

            FileInfo workingFile = new FileInfo($"{_config.WorkingDirectory.FullName}{Path.DirectorySeparatorChar}{_settings.FileNameBase}_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.txt");

            _fileWriter = _writerFactory.CreateWriter(_settings.WriterTypeKey, workingFile);

            MongoCursor cursor = new MongoCursor(query, _settings.Projection, _settings.CollectionName, _connection);

            if (query.Contains("aggregate("))
            {
                int startIndex = query.IndexOf('(') + 1;
                int endIndex = query.LastIndexOf(')');
                cursor.query = query.Substring(startIndex, endIndex - startIndex);

                await cursor.ExecuteAggregateCursor(WriteDataToFile);
            }
            else
            {
                await cursor.ExecuteCursor(WriteDataToFile);
            }

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
