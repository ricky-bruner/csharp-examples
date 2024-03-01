using IntervalProcessing.Processors;

namespace IntervalProcessing
{
    public class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Run() 
        {
            IFileProcessor dailyAuditInventoryProcessor = (DailyAuditInventoryProcessor)_serviceProvider.GetService(typeof(DailyAuditInventoryProcessor));
            dailyAuditInventoryProcessor?.Execute();

        }
    }
}
