using IntervalProcessing.Processors;

namespace IntervalProcessing
{
    public class App
    {
        private readonly IFileProcessorFactory _processorFactory;

        public App(IFileProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public void Run() 
        {
            IFileProcessor dailyAuditInventoryProcessor = _processorFactory.GetProcessor(typeof(DailyAuditInventoryProcessor).Name);
            dailyAuditInventoryProcessor.Execute();

            //IFileProcessor weeklyAuditInventoryProcessor = _processorFactory.GetProcessor("WeeklyAuditInventory");
            //weeklyAuditInventoryProcessor.Execute();
        }
    }
}
