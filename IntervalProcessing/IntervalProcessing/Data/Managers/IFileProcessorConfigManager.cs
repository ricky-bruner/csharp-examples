using IntervalProcessing.Data.Models;

namespace IntervalProcessing.Data.Managers
{
    public interface IFileProcessorConfigManager
    {
        Task<FileProcessorSpecification> GetFileProcessorSpecification(Type processorType);
    }
}
