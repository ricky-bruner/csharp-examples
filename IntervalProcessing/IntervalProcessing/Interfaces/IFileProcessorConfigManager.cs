using IntervalProcessing.Configurations;

namespace IntervalProcessing.Interfaces
{
    public interface IFileProcessorConfigManager
    {
        Task<FileProcessorSpecification?> GetFileProcessorSpecification(Type processorType);
    }
}
