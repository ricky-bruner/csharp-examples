using Microsoft.Extensions.DependencyInjection;

namespace IntervalProcessing.Processors
{
    public class FileProcessorFactory : IFileProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public FileProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFileProcessor GetProcessor(string processorTypeAsString)
        {
            switch (processorTypeAsString) 
            {
                case "DailyAuditInventoryProcessor":
                    return _serviceProvider.GetServices<IFileProcessor>().Where(p => p is DailyAuditInventoryProcessor).FirstOrDefault();

                default: 
                    throw new KeyNotFoundException($"Processor with key {processorTypeAsString} not found.");

            }
        }
    }
}
