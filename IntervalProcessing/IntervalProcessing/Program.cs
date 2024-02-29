using IntervalProcessing.Processors;
using IntervalProcessing.Utilities;
using IntervalProcessing.Writers;
using MongoDB.Bson;
using static IntervalProcessing.Utilities.Constants.DatabaseCollections;

namespace IntervalProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"{DateTime.Now} - Awake...");

                //if (DateTime.Now.Hour == 2)
                {
                    RunProcesses();
                }

                // Sleep for 1 hour (60 minutes * 60 seconds * 1000 milliseconds)
                Console.WriteLine($"{DateTime.Now} - Sleeping...");
                Thread.Sleep(60 * 60 * 1000);
            }
        }

        static void RunProcesses()
        {
            Console.WriteLine($"{ DateTime.Now} - Running scheduled processes...");

            MongoConnection<BsonDocument> connection = new MongoConnection<BsonDocument>(Config.GetMongoConnectionString(), Config.GetDatabase(), StoredQueries);
            ExecuteInventoryFile(connection);

            Console.WriteLine($"{DateTime.Now} - Processes completed.");
        }

        static void ExecuteInventoryFile(MongoConnection<BsonDocument> connection) 
        {
            
            Console.WriteLine($"{DateTime.Now} - Executing Daily Audit Inventory processes...");
            InventoryFileProcessor processor = new InventoryFileProcessor(connection, "dailyAuditInventory", "{}", "DailyAuditInventory", typeof(DailyAuditInventoryFileWriter));
            processor.Execute();
        }
    }
}