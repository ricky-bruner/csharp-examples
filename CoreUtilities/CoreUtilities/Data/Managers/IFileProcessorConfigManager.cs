using CoreUtilities.Data.Models;

namespace CoreUtilities.Data.Managers
{
    public interface IFileProcessorConfigManager
    {
        Task<FileProcessorSpecification> GetFileProcessorSpecification(Type processorType);
    }
}
