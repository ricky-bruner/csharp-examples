using CoreUtilities.Data.Enums;
using CoreUtilities.Processors;
using IntervalProcessing.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace IntervalProcessing
{
    public class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Run(string processToRunKey) 
        {
            switch (processToRunKey)
            {
                case "FileGenerationProcesses":
                    await RunFileGenerationProcesses();
                    break;
                case "NightlyDataChangeProcesses":
                    await RunDataChangeProcesses(RunCadence.Nightly);
                    break;
                case "HourlyDataChangeProcesses":
                    await RunDataChangeProcesses(RunCadence.Hourly);
                    break;
                case "HalfHourlyDataChangeProcesses":
                    await RunDataChangeProcesses(RunCadence.HalfHourly);
                    break;
                case "QuarterHourlyDataChangeProcesses":
                    await RunDataChangeProcesses(RunCadence.QuarterHourly);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task RunFileGenerationProcesses() 
        {
            DailyAuditInventoryProcessor dailyAuditInventoryProcessor = (DailyAuditInventoryProcessor)_serviceProvider.GetService(typeof(DailyAuditInventoryProcessor));
            await dailyAuditInventoryProcessor?.Execute();

            DailyActiveMRRInventoryProcessor dailyActiveMRRInventoryProcessor = (DailyActiveMRRInventoryProcessor)_serviceProvider.GetService(typeof(DailyActiveMRRInventoryProcessor));
            await dailyActiveMRRInventoryProcessor?.Execute();

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                WeeklyAuditInventoryProcessor weeklyAuditInventoryProcessor = (WeeklyAuditInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyAuditInventoryProcessor));
                await weeklyAuditInventoryProcessor?.Execute();

                WeeklyMRRInventoryProcessor weeklyMRRInventoryProcessor = (WeeklyMRRInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyMRRInventoryProcessor));
                await weeklyMRRInventoryProcessor?.Execute();
            }
        }

        private async Task RunDataChangeProcesses(RunCadence runCadence)
        {
            IDataProcessor dataChangeProcessor = (IDataProcessor)_serviceProvider.GetService(typeof(QueryBasedDataUpdateProcessor));
            await dataChangeProcessor.Execute(runCadence);
        }
    }
}
