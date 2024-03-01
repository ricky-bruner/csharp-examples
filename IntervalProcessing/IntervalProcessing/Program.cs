using IntervalProcessing.Interfaces;
using IntervalProcessing.Processors;
using IntervalProcessing.Utilities;
using IntervalProcessing.Writers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} - Initiating IntervalProcessing Processes...");

            //if (DateTime.Now.Hour == 2)
            {
                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                App app = serviceProvider.GetService<App>();
                app.Run();
            }

            Console.WriteLine($"{DateTime.Now} - IntervalProcessing Complete...");
        }

        private static void ConfigureServices(IServiceCollection services) 
        {
            services.AddSingleton(typeof(IMongoConnection<BsonDocument>), provider => 
                new MongoConnection<BsonDocument>(CoreConfig.GetMongoConnectionString(), CoreConfig.GetDatabase(), StoredQueries));
            services.AddSingleton<IWriterFactory, WriterFactory>();
            services.AddSingleton<IStoredQueryManager, StoredQueryManager>();
            services.AddSingleton<IFileProcessorConfigManager, FileProcessorConfigManager>();

            services.AddTransient<IFileProcessor, DailyAuditInventoryProcessor>();

            services.AddTransient<App>();
        }
    }
}