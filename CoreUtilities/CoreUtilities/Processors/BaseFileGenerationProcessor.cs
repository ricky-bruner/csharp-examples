using CoreUtilities.Writers;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Data.Models;
using MongoDB.Bson;
using CoreUtilities.CloudServices.AWS;

namespace CoreUtilities.Processors
{
    public abstract class BaseFileGenerationProcessor : IFileProcessor
    {
        private IMongoConnection<BsonDocument> _connection { get; set; }
        private IConfig _config { get; set; }
        private FileProcessorSpecification _settings { get; set; }
        private readonly IWriterFactory _writerFactory;
        private IWriter<BsonDocument>? _fileWriter { get; set; }
        private IStoredQueryManager _queryManager { get; set; }
        private IS3Uploader _s3Uploader { get; set; }
        private readonly Type _type;

        public BaseFileGenerationProcessor(IMongoConnection<BsonDocument> connection, IConfig config, IFileProcessorConfigManager fileProcessorConfigManager, Type processorType, IWriterFactory writerFactory, IStoredQueryManager queryManager, IS3Uploader s3Uploader)
        {
            _writerFactory = writerFactory;
            _connection = connection;
            _config = config;
            _queryManager = queryManager;
            _s3Uploader = s3Uploader;
            _type = processorType;
            _settings = fileProcessorConfigManager.GetFileProcessorSpecification(processorType).Result;
        }

        public virtual async Task Execute()
        {
            Console.WriteLine($"{DateTime.Now} - Executing {_type.Name} process...");

            string query = await _queryManager.GetQueryAsync(_settings.QueryName);

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

            //future logic for sftp delivery and database detail storage
            await _s3Uploader.UploadFileAsync("GeneratedFiles", workingFile.FullName);

            Console.WriteLine($"{DateTime.Now} - {_type.Name} process complete...");
        }

        private void WriteDataToFile(BsonDocument document)
        {
            _fileWriter?.Write(ApplyCustomLogic(document));
        }

        public abstract BsonDocument ApplyCustomLogic(BsonDocument document);
    }
}
