using CoreUtilities.CloudServices.AWS;
using CoreUtilities.CloudServices.Utilities;
using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
using CoreUtilities.Processors;
using CoreUtilities.Writers;
using IntervalProcessing.Processors;
using IntervalProcessing.Writers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using static CoreUtilities.Constants.DatabaseCollections;

namespace IntervalProcessing
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} - Initiating IntervalProcessing Processes...");

            ServiceCollection serviceCollection = new ServiceCollection();

            // If running local:
            //string processToRunKey = "FileGenerationProcesses";

            // If running aws:
            string processToRunKey = Environment.GetEnvironmentVariable("processToRun");

            switch (processToRunKey)
            {
                case "FileGenerationProcesses":
                    ConfigureFileGenerationServices(serviceCollection);
                    break;
                case "NightlyDataChangeProcesses":
                case "HourlyDataChangeProcesses":
                case "HalfHourlyDataChangeProcesses":
                case "QuarterHourlyDataChangeProcesses":
                    ConfigureDataChangeServices(serviceCollection);
                    break;
                default:
                    throw new NotImplementedException();
            }

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            App app = serviceProvider.GetService<App>();
            await app.Run(processToRunKey);

            Console.WriteLine($"{DateTime.Now} - IntervalProcessing Complete...");
        }

        private static void ConfigureFileGenerationServices(IServiceCollection services) 
        {
            // configuration settings
            CoreConfig coreConfig = new CoreConfig("config.json");
            services.AddSingleton<IConfig>(coreConfig);

            // database connection
            services.AddSingleton<IMongoConnection<BsonDocument>>(provider =>
                new MongoConnection<BsonDocument>(provider.GetRequiredService<IConfig>(), StoredQueries));

            // factory and manager singletons
            services.AddSingleton<IWriterFactory, WriterFactory>();
            services.AddSingleton<IStoredQueryManager, StoredQueryManager>();
            services.AddSingleton<IFileProcessorConfigManager, FileProcessorConfigManager>();
            services.AddSingleton<IAWSSettingsManager, AWSSettingsManager>();
            services.AddSingleton<IS3Uploader, S3Uploader>();
            services.AddSingleton<IGeneratedFileUploader, GeneratedFileUploader>();

            // processor transients
            services.AddTransient<DailyAuditInventoryProcessor>();
            services.AddTransient<WeeklyAuditInventoryProcessor>();
            services.AddTransient<DailyActiveMRRInventoryProcessor>();
            services.AddTransient<WeeklyMRRInventoryProcessor>();

            // app transient
            services.AddTransient<App>();
        }

        private static void ConfigureDataChangeServices(IServiceCollection services)
        {
            // configuration settings
            CoreConfig coreConfig = new CoreConfig("config.json");
            services.AddSingleton<IConfig>(coreConfig);

            // database connection
            services.AddSingleton<IMongoConnection<BsonDocument>>(provider =>
                new MongoConnection<BsonDocument>(provider.GetRequiredService<IConfig>(), DataUpdateConfigs));

            //manager singletons
            services.AddSingleton<IQueryAutomationManager, QueryAutomationManager>();

            // processor transients
            services.AddTransient<QueryBasedDataUpdateProcessor>();
            // app transient
            services.AddTransient<App>();
        }
    }
}