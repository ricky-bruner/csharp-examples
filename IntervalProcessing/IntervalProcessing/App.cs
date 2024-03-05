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

        public async Task Run() 
        {
            DailyAuditInventoryProcessor dailyAuditInventoryProcessor = (DailyAuditInventoryProcessor)_serviceProvider.GetService(typeof(DailyAuditInventoryProcessor));
            await dailyAuditInventoryProcessor?.Execute();

            WeeklyAuditInventoryProcessor weeklyAuditInventoryProcessor = (WeeklyAuditInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyAuditInventoryProcessor));
            await weeklyAuditInventoryProcessor?.Execute();

            DailyActiveMRRInventoryProcessor dailyActiveMRRInventoryProcessor = (DailyActiveMRRInventoryProcessor)_serviceProvider.GetService(typeof(DailyActiveMRRInventoryProcessor));
            await dailyActiveMRRInventoryProcessor?.Execute();

            WeeklyMRRInventoryProcessor weeklyMRRInventoryProcessor = (WeeklyMRRInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyMRRInventoryProcessor));
            await weeklyMRRInventoryProcessor?.Execute();
        }
    }
}
