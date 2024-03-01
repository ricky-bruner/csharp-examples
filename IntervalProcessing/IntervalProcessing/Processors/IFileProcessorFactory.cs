
namespace IntervalProcessing.Processors
{
    public interface IFileProcessorFactory
    {
        IFileProcessor GetProcessor(string key);
    }
}
