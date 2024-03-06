using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Managers;
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

            string processToRunKey = args.Length > 0 ? args[0] : "default";

            switch (processToRunKey)
            {
                case "FileGenerationProcesses":
                    ConfigureFileGenerationServices(serviceCollection);
                    break;
                case "NightlyDataChangeProcesses":
                    ConfigureNightlyDataChangeProcesses(serviceCollection);
                    break;
                case "HourlyDataChangeProcesses":
                    ConfigureHourlyDataChangeProcesses(serviceCollection);
                    break;
                case "15MinuteDataChangeProcesses":
                    ConfigureFifteenMinuteDataChangeProcesses(serviceCollection);
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

            // processor transients
            services.AddTransient<DailyAuditInventoryProcessor>();
            services.AddTransient<WeeklyAuditInventoryProcessor>();
            services.AddTransient<DailyActiveMRRInventoryProcessor>();
            services.AddTransient<WeeklyMRRInventoryProcessor>();

            // app transient
            services.AddTransient<App>();
        }

        private static void ConfigureNightlyDataChangeProcesses(IServiceCollection services)
        { 
        
        }

        private static void ConfigureHourlyDataChangeProcesses(IServiceCollection services)
        { 
        
        }

        private static void ConfigureFifteenMinuteDataChangeProcesses(IServiceCollection services)
        { 
        
        }
    }
}